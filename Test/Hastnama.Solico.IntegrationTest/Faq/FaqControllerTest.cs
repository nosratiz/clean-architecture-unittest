using FluentAssertions;
using Hastnama.Solico.Application.Cms.Faqs.Command.Create;
using Hastnama.Solico.Application.Cms.Faqs.Dto;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Helper.Pagination;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Hastnama.Solico.IntegrationTest.Faq
{
    public class FaqControllerTest : IntegrationTest
    {

        [Theory]
        [InlineData(1000)]
        [InlineData(2000)]
        public async Task GetInvalidFaqId_returnBadRequest(int id)
        {
            await GetAuthorizationAsync();

            var response = await _httpClient.GetAsync($"/admin/Faqs/{id}");
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            (await response.Content.ReadAsAsync<ApiMessage>()).Message.Should().Be("سوال مورد نظر پیدا نشد");
        }


        [Fact]
        public async Task WhenCreateNewFaq_returnNewFaq()
        {
            await GetAuthorizationAsync();

            var response = await _httpClient.PostAsJsonAsync("/admin/faqs", new CreateFaqCommand
            {
                Answer = "salam",
                Question = "salam"
            });

            response.StatusCode.Should().Be(HttpStatusCode.Created);

            (await response.Content.ReadAsAsync<FaqDto>()).Question.Should().Be("salam");
        }

        [Fact]
        public async Task GetAllFaqs_returnNonEmptyResponse()
        {
            await GetAuthorizationAsync();
            var response = await _httpClient.GetAsync("/admin/Faqs");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            (await response.Content.ReadAsAsync<PagedList<FaqDto>>()).Items.Should().NotBeNull();
        }
    }
}