using System.IO;
using System.Net.Http;
using AiclaRM.Server.Services.ECommerce;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkiaSharp;

namespace AiclaRM.Server.Extensions
{
    public static class CatalogApiExtensions
    {
        public static void MapCatalogAPI(this WebApplication app)
        {
            var catalogApi = app.MapGroup("/api/v1/catalog");

            catalogApi.MapGet("/catalogBrands", async ([FromServices]  ICatalogService catalogService) =>
            {
                var brands = await catalogService.GetCatalogBrandsAsync();
                return Results.Ok(brands);
            })
            .WithTags("RM :  Basic");

            catalogApi.MapGet("/catalogTypes", async ([FromServices] ICatalogService catalogService) =>
            {
                var types = await catalogService.GetCatalogProductTypesAsync();
                return Results.Ok(types);
            })
            .WithTags("RM :  Basic");

            catalogApi.MapGet("/products", async ([FromServices] ICatalogService catalogService, int pageIndex = 0, int pageSize = 10) =>
            {
                var products = await catalogService.GetCatalogProductsAsync(pageIndex, pageSize);
                return Results.Ok(products);
            })
            .WithTags("RM :  Basic");

            catalogApi.MapGet("/products/{id:int}", async ([FromServices] ICatalogService catalogService, int id) =>
            {
                var product = await catalogService.GetProductByIdAsync(id);
                return product == null ? Results.NotFound() : Results.Ok(product);
            })
            .WithTags("RM :  Basic");

            catalogApi.MapGet("/products/{catalogProductId:int}/pic", async (
                [FromServices] ICatalogService catalogService,
                int catalogProductId,
                [FromServices] IHttpClientFactory httpClientFactory) =>
            {
                var product = await catalogService.GetProductByIdAsync(catalogProductId);
                if (product == null)
                    return Results.NotFound();

                var photoUrl = product.photo_url;
                var client = httpClientFactory.CreateClient();
                var imageBytes = await client.GetByteArrayAsync(photoUrl);

                using var inputStream = new MemoryStream(imageBytes);
                using var original = SKBitmap.Decode(inputStream);
                var resized = original.Resize(new SKImageInfo(190, 190), SKFilterQuality.Medium);

                if (resized == null)
                    return Results.Problem("Image resizing failed", statusCode: 500);

                using var image = SKImage.FromBitmap(resized);
                using var outputStream = new MemoryStream();
                image.Encode(SKEncodedImageFormat.Webp, 100).SaveTo(outputStream);

                return Results.File(outputStream.ToArray(), "image/webp");
            })
            .WithTags("RM :  Basic");
        }
    }
}
