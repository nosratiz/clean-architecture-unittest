using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Application.Files.Dto;
using Hastnama.Solico.Common.Environment;
using Hastnama.Solico.Common.FileProcessor;
using Hastnama.Solico.Common.Options;
using Hastnama.Solico.Common.Result;
using Hastnama.Solico.Domain.Models.UserManagement;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Serilog;

namespace Hastnama.Solico.Application.Files.Command
{
    public class CreateFileCommandHandler : IRequestHandler<CreateFileCommand, Result<FileDto>>
    {
        private readonly ISolicoDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        private readonly ImageProcessor _imageProcessor;
        private readonly IOptionsMonitor<ImageSetting> _imageSetting;


        public CreateFileCommandHandler(ISolicoDbContext context, IMapper mapper, IWebHostEnvironment env,
            ImageProcessor imageProcessor, IOptionsMonitor<ImageSetting> imageSetting)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
            _imageProcessor = imageProcessor;
            _imageSetting = imageSetting;
        }

        public async Task<Result<FileDto>> Handle(CreateFileCommand request, CancellationToken cancellationToken)
        {
            var tempPath = Path.Combine(ApplicationStaticPath.Documents, request.Files.FileName);
            var uniqueId = $"{Guid.NewGuid():N}";
            var fileName = Path.GetFileNameWithoutExtension(tempPath);
            var extension = Path.GetExtension(tempPath);
            var newName = $"{uniqueId}{extension}";
            var filePath = Path.Combine(_env.ContentRootPath, ApplicationStaticPath.Documents, newName);

            #region Save To File

            await using var fileStream = new FileStream(filePath, FileMode.Create);

            await request.Files.CopyToAsync(fileStream, cancellationToken);

            try
            {
                var uploadImage = _imageProcessor.Crop(request.Files, _imageSetting.CurrentValue.Width,
                    _imageSetting.CurrentValue.Height);

                _imageProcessor.Save(uploadImage,
                    Path.Combine(_env.ContentRootPath, ApplicationStaticPath.Documents, $"{uniqueId}_n{extension}"),
                    $"{fileName}_n", _imageSetting.CurrentValue.Quality);
            }
            catch (Exception e)
            {
                Log.Error(e.Message, e.StackTrace);
                
                await using var newFileStream = new FileStream(Path.Combine(_env.ContentRootPath, ApplicationStaticPath.Documents, $"{uniqueId}_n{extension}"), FileMode.Create);

                await request.Files.CopyToAsync(newFileStream, cancellationToken);
            }

            #endregion

            #region Save To Database

            var file = new UserFile()
            {
                Type = request.Type,
                Name = fileName,
                Size = request.Files.Length,
                Url = $"{ApplicationStaticPath.Clients.Document}/{newName}",
                MediaType = request.Files.ContentType,
                Path = $"/{ApplicationStaticPath.Documents}{newName}",
                CreateDate = DateTime.Now,
                UniqueId = Guid.NewGuid().ToString("N")
            };

            await _context.UserFiles.AddAsync(file, cancellationToken);

            await _context.SaveAsync(cancellationToken);

            #endregion

            return Result<FileDto>.SuccessFul(_mapper.Map<FileDto>(file));
        }
    }
}