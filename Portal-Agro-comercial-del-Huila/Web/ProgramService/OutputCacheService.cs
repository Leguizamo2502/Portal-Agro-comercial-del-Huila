namespace Web.ProgramService
{
    public static class OutputCacheService
    {
        public static IServiceCollection AddOutputCachePolicies(this IServiceCollection services)
        {
            services.AddOutputCache(options =>
            {
                // Lista de productos pública (paginada/filtrada)
                options.AddPolicy("ProductsListPolicy", p => p
                    .Expire(TimeSpan.FromSeconds(60))
                    .SetVaryByQuery("page", "pageSize", "q", "categoryId", "sort", "producerCode", "limit")
                    .Tag("products"));

                // Detalle de producto por ID (varía por ruta)
                options.AddPolicy("ProductDetailPolicy", p => p
                    .Expire(TimeSpan.FromMinutes(5))
                    .SetVaryByRouteValue("id")
                    .Tag("products"));

                // Productos por categoría
                options.AddPolicy("CategoryProductsPolicy", p => p
                    .Expire(TimeSpan.FromSeconds(60))
                    .SetVaryByRouteValue("categoryId")
                    .SetVaryByQuery("page", "pageSize", "sort", "q")
                    .Tag("products"));

                // Home (listado corto)
                options.AddPolicy("HomeProductsPolicy", p => p
                    .Expire(TimeSpan.FromSeconds(60))
                    .SetVaryByQuery("limit")
                    .Tag("products"));

                // Destacados
                options.AddPolicy("FeaturedProductsPolicy", p => p
                    .Expire(TimeSpan.FromSeconds(60))
                    .SetVaryByQuery("limit")
                    .Tag("products"));
            });

            return services;
        }
    }
}
