using CSharpEssentials.Interfaces;
using MuhammedTask.Services.CategoryService.Domain.Categories.Fields;

namespace MuhammedTask.Services.CategoryService.Domain.Categories.Events;

public sealed record CategoryDeletedDomainEvent(CategoryId Id) : IDomainEvent;


