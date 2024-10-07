using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public List<MapTile> mapTiles; // The main list of tiles where all mapTile data lies
    public List<GameObject> gameTiles; // The main list of physical GameObjects which represents tiles
    public int normalTiles {get; private set;} // The amount of normal tiles in the game
    public int lastMainNormalTileIndex {get; private set;} // the last tile before looping back to the starting position 
    public int tilesPerSide{get; private set;} // The amount of tiles on each side, excluding corner tiles
    public int outerTilesLength {get; private set;} // The amount of outer tiles on the map
    public int middleTilesLength {get; private set;} // The amount of middle tiles on the map
    public int specialTiles {get; private set;} // Kinda redundant right now but could be useful in the future
    public int specialTileIndexOffset {get; private set;} // the offset to be centered in the middle of the map 
    public int tradeTileIndexOffset {get; private set;} // The offset to be centered on the four sides
    public int totalTiles {get; private set;} // The total amount of tiles generated
    [SerializeField] private GameObject prefabObject; // The object template to represent the tiles that we can clone into a lists
    [SerializeField] private Transform mapHostTransform; // The transform which parents all tile gameobject representations
    [SerializeField] private Camera camera; // The camera showing the birds eye view of the game. This is done here simply because the variable it needs is here
    public Transform GetMapHostTransform {get {return mapHostTransform;} } // Allows other scripts to get mapHostTransform
    [SerializeField] public Sprite[] tileSprites;
    public static MapManager main {get; private set;} 

    // Start is called before the first frame update
    void Awake()
    {
        // Initializing Singleton variable
        if(main == null){main = this;}
        else if(main != this){Destroy(this);}

        // Initializing essential map tile pools
        mapTiles = new List<MapTile>();
        gameTiles = new List<GameObject>();

        // Note: Tiles per side variable does not include the corner tiles. 
        // The sides therefore will actually be 2 tiles wider than what is specified here

        // TilesPerSide must be an odd number in order to properly generate in compliance with middle tiles
        // TilesPerSide must be at least 3
        tilesPerSide = 7; // Default value
        // GenerateMap();
        

        // SetMapGenerationVariables();

    }

    public MapTile GetMiddleTile(bool entrance = true){
        foreach (MapTile tile in mapTiles){
            if (tile.middleConnectorType == 1 && !entrance){
                return tile;}
            else if (tile.middleConnectorType == 0 && entrance){
                return tile;}
        }
        return null;
    }

    public MapTile GetTile(int index){
        foreach (MapTile tile in mapTiles){
            if (tile.index == index){return tile;}
        }

        return null;
    }

    public List<MapTile> GetNormalTiles(){
        List<MapTile> result = new List<MapTile>();

        foreach (MapTile tile in mapTiles){
            if (tile.tileType == "_normal"){result.Add(tile);}
        }

        return result;
    }
    public List<MapTile> GetSpecialTiles(){
        List<MapTile> result = new List<MapTile>();

        foreach (MapTile tile in mapTiles){
            if (tile.tileType == "_special"){result.Add(tile);}
        }

        return result;
    }

    public int GetSpotsPastMiddleConnector(int position){ // Only works for outer tiles
        MapTile tile = GetMiddleTile();
        if(tile != null){
            return (position - tile.index);
        }

        return 1000000;
    }

    void SetMapGenerationVariables(){
        specialTiles = 1; // Kinda redundant right now but could be useful in the future

        // 1st term represents the row of tiles going across the very middle of the screen
        // 2nd term represents the two columns of tiles connecting the middle row's endpoints back to the normal boards
        middleTilesLength = (tilesPerSide - 2 ) + (tilesPerSide - 1);

        // Middle tiles will always be odd. This special tile will always be at the midpoint of the middle tiles
        specialTileIndexOffset = (int)(middleTilesLength/2);
        tradeTileIndexOffset = (int)(tilesPerSide/2)+1;
        
        // The amount of normal tiles in the map
        normalTiles = (4 * tilesPerSide) + (middleTilesLength - 1);
    }

    public void SetMapSize(int size){
        tilesPerSide = size;
    }

    // Generates the main map with the given variables
    public void GenerateMap(){
        if(mapTiles == null){ // In case the tile list for some reason does not exist, avoid execution entirely
            Debug.Log("The map tile list has not been initiated. Avoiding map generation...");
            return;
        }

        SetMapGenerationVariables();
        Debug.Log("Set map generation variables");


        /* ------------------ Generates Map tile CLASS OBJECTS ------------------- */

        mapTiles.Clear(); // Clears any previous map tile data
        int assignedIndex = 0; // Assigns a unique index for each new generated tile
        
        mapTiles.Add(new MapTile(assignedIndex, "_startingPosition"));
        assignedIndex++;
        
        for(int i = 0; i < 4; i++){ // sides generating loop, i represents the (n)th side on the table 

            for(int j = 0; j < tilesPerSide; j++){ // Each tile generating loop. J represents the (n)th normal tile per side 
                
                switch(i){ // Checks to add the required middle connector attributes
                    case 0:
                        if(j == tilesPerSide-2){ mapTiles.Add(new MapTile(assignedIndex, "_normal", 0)); } // The first middle connector tile
                        else{ 
                            if(j > tilesPerSide-2 && Random.Range(0, 5) == 1){ 
                                mapTiles.Add(new MapTile(assignedIndex, "_special")); 
                                continue;}
                            mapTiles.Add(new MapTile(assignedIndex, "_normal")); }
                        
                        break;

                    case 1:
                        if( Random.Range(0, 5) == 1){  mapTiles.Add(new MapTile(assignedIndex, "_special"));  }
                        else{mapTiles.Add(new MapTile(assignedIndex, "_normal"));}
                        break;
                        
                    case 2:
                        if(j == tilesPerSide-2){ mapTiles.Add(new MapTile(assignedIndex, "_normal", 1)); } // The second middle connector tile
                        else if(j < tilesPerSide-2 && Random.Range(0, 5) == 1){  mapTiles.Add(new MapTile(assignedIndex, "_special"));  }
                        else{ mapTiles.Add(new MapTile(assignedIndex, "_normal")); }

                        break;
                    default:
                        mapTiles.Add(new MapTile(assignedIndex, "_normal"));
                        break;
                }


                // Always adds one to the index that should be assigned, since all cases add another map tile
                assignedIndex++;

            }

            // At the end of each side
            if(i != 3){
                // Declares a corner tile and indexes them via concatenation for i
                // mapTiles.Add(new MapTile(assignedIndex, "_corner"+i.ToString() ));
                mapTiles.Add(new MapTile(assignedIndex, "_normal" ));
                assignedIndex++;
            }
            else{
                // lastMainNormalTileIndex = assignedIndex-1;
                outerTilesLength = assignedIndex;
                Debug.Log("Completed loop to first tile | Outer Map Generated");


            }
        }

        for(int i = 0; i < middleTilesLength; i++){ // Loop for creating middle tile objects
            
            // Adds normal tiles except for the very middle tile, which is special
            if(i == specialTileIndexOffset)
            { mapTiles.Add(new MapTile(assignedIndex, "_special")); }
            else
            { mapTiles.Add(new MapTile(assignedIndex, "_normal")); }

            assignedIndex++;
        }

        totalTiles = assignedIndex;
        Debug.Log("Middle Map Generated | Map tile CLASS OBJECT generation complete!");

        /* ------------------ Generates Map tile GAME OBJECTS ------------------- */

        // Side units are Always odd
        int sideUnits = tilesPerSide+2; // This is the tile of a side including corners
        int sideUnitsMidpoint = sideUnits/2 + 1; // The midpoint offset of each side
        float gridUnit = 2f; // The spacing between each object

        // Destroy gameTiles object
        for (int i = 0; i < gameTiles.Count; i++)
        { Destroy(gameTiles[i]); }

        gameTiles.Clear();
        
        // Outer map
        for(int i = 0; i < 4; i++){
            for(int j = 0; j < sideUnits-1; j++){

                GameObject tileObj = Instantiate(prefabObject, mapHostTransform);
                gameTiles.Add(tileObj);

                switch (i) {
                    case 0:

                        tileObj.transform.position = new Vector3( j*gridUnit ,  i == 0 ? 0 : sideUnits*gridUnit, 0);
                        break;
                    case 1:
                        tileObj.transform.position = new Vector3( 
                            (sideUnits-1)*gridUnit , 
                            j*gridUnit , 
                            0
                        );

                        break;
                    case 2:
                        tileObj.transform.position = new Vector3( 
                            (gridUnit * (sideUnits-1)) - (j)*gridUnit ,
                            (sideUnits-1)*gridUnit,
                            0
                        );

                        break;
                    case 3:
                        tileObj.transform.position = new Vector3( 
                            0,
                            (gridUnit * (sideUnits-1) ) - (j)*gridUnit,
                            0
                        );

                        break;
                    default:
                        break;
                }

            }
        }

        Debug.Log("Outer Map Generated | Map tile gameObject generation-");

        // Middle map
        for (int i = 0; i < tilesPerSide; i++)
        {
            GameObject tileObj;


            if(i < sideUnitsMidpoint-2){ // Bottom left mid

                tileObj = Instantiate(prefabObject, mapHostTransform);
                tileObj.transform.position = new Vector3( (sideUnits-3)*gridUnit,  (i+1)*gridUnit, 0);
            
            }
            else if (i > sideUnitsMidpoint-2){ // Top right mid

                tileObj = Instantiate(prefabObject, mapHostTransform);
                tileObj.transform.position = new Vector3( ((tilesPerSide+1) *gridUnit) - ((tilesPerSide-1) * gridUnit),  (i+1)*gridUnit, 0);
            
            }
            else{
                for (int j = (tilesPerSide-3); j >= 0; j--){ // Horizontal across
                    tileObj = Instantiate(prefabObject, mapHostTransform);
                    tileObj.transform.position = new Vector3(((j+2) * gridUnit),  (sideUnitsMidpoint-1)*gridUnit, 0);
                    gameTiles.Add(tileObj);
                    continue;
                }

                continue;
            }
            gameTiles.Add(tileObj);
        }

        Debug.Log("Middle Map Generated | Map tile GAMEOBJECT generation complete...");

        // Assigning gameObjects to maptile objects
        

        for (int i = 0; i < mapTiles.Count; i++)
        {
            mapTiles[i].SetGameObject(gameTiles[i]);
        }
        
        Debug.Log("Assigned gameObjects to map tile object...");

        camera.transform.position = new Vector3( tilesPerSide+1, tilesPerSide+1, -10);
        camera.orthographicSize= tilesPerSide+5;

        SetMapTiles();
    }

    public void SetMapTiles(){
        List<MapTile> normalTiles = GetNormalTiles();
        List<MapTile> specialTiles= GetSpecialTiles();

        int totalCount = normalTiles.Count;
		
		for (int i = 0; i < totalCount-1; i++) {
			int j = Random.Range(i, totalCount);
			MapTile tile = normalTiles[i];
			normalTiles[i] = normalTiles[j];
			normalTiles[j] = tile;
		}
        int index = 0;
        for (int i = 0; i < normalTiles.Count; i++)
        {
            normalTiles[i].SetTileType(index);
            index++;
            if(index == 4){ index = 0; }
        }

        foreach (MapTile special in specialTiles)
        {
            special.SetTileType(5);
        }

	}

    public void SetNormalTilesPerSide(int _tilesPerSide){
        if( ((float)_tilesPerSide / 2.0f) == 0 ){
            _tilesPerSide++; // Makes the count odd if it is currently even
        } 
        // Sets the tiles per side to 3 if it is below that number, since any value below 3 is out of its functioning range
        if(_tilesPerSide < 3){ _tilesPerSide = 3; } 

        tilesPerSide = _tilesPerSide;
        GenerateMap();
    }

}
