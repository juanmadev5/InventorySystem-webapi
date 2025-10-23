namespace InventorySystem_webapi.Constants {
    public static class InventoryMessages
    {
        // Mensajes de error para GetProductByCode
        public const string ProductNotFound = "Product not found";

        // Mensajes de error para GetProductsByCategory
        public const string CategoryNameRequired = "Category name is required.";
        public const string NoProductsInCategoryFormat = "No products found in category '{0}'.";

        // Mensajes de error para UpdateProduct, DeleteProduct
        public const string ProductWithCodeNotFoundFormat = "Product with code '{0}' not found.";

        // Mensaje de éxito para UpdateProduct
        public const string ProductUpdatedSuccessfullyFormat = "Product '{0}' updated successfully.";
        
        // Mensaje de éxito para DeleteProduct
        public const string ProductDeletedSuccessfullyFormat = "Product '{0}' deleted successfully.";
    }
}