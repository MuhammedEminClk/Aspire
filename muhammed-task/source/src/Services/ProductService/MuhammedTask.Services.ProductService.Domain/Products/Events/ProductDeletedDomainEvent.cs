using CSharpEssentials.Interfaces;
using MuhammedTask.Services.ProductService.Domain.Products.Fields;

namespace MuhammedTask.Services.ProductService.Domain.Products.Events;

public sealed record ProductDeletedDomainEvent(ProductId Id) : IDomainEvent;


