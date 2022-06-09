using System.Threading.Tasks;

namespace Hastnama.Solico.Common.Email
{
    public interface IViewRenderServices
    {
        Task<string> RenderToStringAsync(string viewName, object model);
    }
}