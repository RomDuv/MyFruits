using Microsoft.AspNetCore.Mvc.Routing;
using MyFruits.Options;

namespace MyFruits.Services;

public class PathService
{
    private readonly IConfiguration configuration;
    private readonly IWebHostEnvironment env; 
    public PathService(IConfiguration configuration, IWebHostEnvironment env)
    {
        this.configuration = configuration; 
        this.env = env;
    }

    public string GetUploadsPath(string? filename=null, bool withWebPath = true)
    {
        var pathOptions = new PathOptions();
        configuration.GetSection(PathOptions.Path).Bind(pathOptions);

        var UploadsPath = pathOptions.FruitsImages;

        if(null!=filename)
        {
            UploadsPath=Path.Combine(UploadsPath, filename);
        }
        return withWebPath ? Path.Combine(env.WebRootPath, UploadsPath) : UploadsPath;
    }
}
