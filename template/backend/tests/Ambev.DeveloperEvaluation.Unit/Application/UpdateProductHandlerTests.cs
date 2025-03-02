using Ambev.DeveloperEvaluation.Application.DTOs;
using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    public class UpdateProductHandlerTests
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly UpdateProductHandler _handler;

        public UpdateProductHandlerTests()
        {
            _productRepository = Substitute.For<IProductRepository>();
            _mapper = Substitute.For<IMapper>();
            _handler = new UpdateProductHandler(_productRepository, _mapper);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsUpdatedProductResult()
        {
            // Arrange
            var command = UpdateProductHandlerTestData.GenerateValidCommand();

            var product = new Product
            {
                Id = command.Id,
                Title = command.Title,
                Price = command.Price,
                Description = command.Description,
                Category = command.Category,
                Image = command.Image,
                Rating = new Rating { Count = command.Rating.Count, Rate = command.Rating.Rate}
            };

            var updatedProduct = new Product
            {
                Id = command.Id,
                Title = command.Title,
                Price = command.Price,
                Description = command.Description,
                Category = command.Category,
                Image = command.Image,
                Rating = new Rating { Count = command.Rating.Count, Rate = command.Rating.Rate }
            };

            var productResult = new UpdateProductResult
            {
                Id = command.Id,
                Title = command.Title,
                Price = command.Price,
                Description = command.Description,
                Category = command.Category,
                Image = command.Image,
                Rating = command.Rating,
            };

            _mapper.Map<Product>(Arg.Any<UpdateProductCommand>()).Returns(product);
            _productRepository.UpdateAsync(Arg.Any<int>(), Arg.Any<Product>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(updatedProduct));
            _mapper.Map<UpdateProductResult>(Arg.Any<Product>()).Returns(productResult);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(command.Id, result.Id);
            Assert.Equal(command.Title, result.Title);
            Assert.Equal(command.Price, result.Price);
            Assert.Equal(command.Description, result.Description);
            Assert.Equal(command.Category, result.Category);
            Assert.Equal(command.Image, result.Image);
            Assert.Equal(command.Rating.Rate, result.Rating.Rate);
            Assert.Equal(command.Rating.Count, result.Rating.Count);
        }

        [Fact(DisplayName = "Given invalid command, When handling, Then throws ValidationException")]
        public async Task Handle_InvalidCommand_ThrowsValidationException()
        {
            // Arrange
            var command = new UpdateProductCommand
            {
                Id = 0, // ID inválido
                Title = "", // Título inválido
                Price = -1, // Preço inválido
                Description = "", // Descrição inválida
                Category = "", // Categoria inválida
                Image = "", // Imagem inválida
                Rating = new RatingDto
                {
                    Rate = -1, // Nota inválida
                    Count = -1 // Contagem inválida
                }
            };

            var validator = new UpdateProductCommandValidator();
            var validationResult = await validator.ValidateAsync(command);

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage($"*{validationResult.Errors[0].ErrorMessage}*");
        }

        [Fact(DisplayName = "Given repository throws exception, When handling, Then throws exception")]
        public async Task Handle_RepositoryThrowsException_ThrowsException()
        {
            // Arrange
            var command = UpdateProductHandlerTestData.GenerateValidCommand();
            var product = new Product();

            _mapper.Map<Product>(command).Returns(product);
            _productRepository.UpdateAsync(command.Id, product, Arg.Any<CancellationToken>())
                .Throws(new Exception("Error updating product"));

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Error updating product");
        }

        [Fact(DisplayName = "Given mapper throws exception, When handling, Then throws exception")]
        public async Task Handle_MapperThrowsException_ThrowsException()
        {
            // Arrange
            var command = UpdateProductHandlerTestData.GenerateValidCommand();
            var product = new Product();

            _mapper.Map<Product>(command).Returns(product);
            _productRepository.UpdateAsync(command.Id, product, Arg.Any<CancellationToken>()).Returns(product);
            _mapper.Map<UpdateProductResult>(product).Throws(new Exception("Error mapping product"));

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Error mapping product");
        }
    }
}
