# Azure Functions
Copy the contents of a given file into an Azure Function.

## [Get random cat image](/Functions/GetRandomCatImage.cs)
**Trigger:** HTTP  
**Purpose:** Demonstrate *Dependencies* and *Application map* in *Application Insights*  

**Description:**
1. Queries [The cat API](https://thecatapi.com/) for random cat image URL. 
2. Gets image stream from URL
3. Returns image