# AdventureWorksExercise
AdventureWorks API exercise

The API is built using OData for ASP.NET Core

Some links to try:

https://localhost:7194/api/v1/products?$top=1
https://localhost:7194/api/v1/products?$filter=contains(productsubcategory/name,%27Wheels%27)&$top=3
https://localhost:7194/api/v1/products?$orderby=productsubcategory/productcategory/name%20desc&$top=5
https://localhost:7194/api/v1/products?$orderby=ListPrice%20desc&$top=10&$select=productid,name,listprice

OData query information:
https://docs.microsoft.com/en-us/odata/concepts/queryoptions-overview
