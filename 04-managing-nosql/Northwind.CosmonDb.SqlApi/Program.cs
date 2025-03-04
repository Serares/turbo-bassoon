// await CreateCosmosResources();

// await CreateProductItems();

// await ListProductItems("SELECT p.id, p.productName, p.unitPrice FROM Items p where p.category.categoryName = 'Beverages'");

// await DeleteProductItems();

// await CreateInsertProductStoredProcedure();

await ExecuteInsertProductStoredProcedure();
