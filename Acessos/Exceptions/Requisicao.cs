using Microsoft.AspNetCore.Mvc;

namespace Acessos.Exceptions
{
    public static class Requisicao
    {
        /// <summary>
        /// Trata com a execução de uma função fornecida e padroniza o tratamento de exceções.
        /// </summary>
        /// <param name="func">A função a ser executada, que retorna um IActionResult.</param>
        /// <returns>
        /// Retorna o resultado da execução da função ou uma resposta de erro apropriada se ocorrer uma exceção.
        /// </returns>
        /// <remarks>
        /// Este método captura exceções específicas e retorna respostas HTTP padronizadas:
        /// <list type="bullet">
        /// <item><description>ArgumentException: Retorna um BadRequest (HTTP 400) com a mensagem da exceção.</description></item>
        /// <item><description>KeyNotFoundException: Retorna um NotFound (HTTP 404) com a mensagem da exceção.</description></item>
        /// <item><description>Exception: Retorna um StatusCode (HTTP 500) com uma mensagem de erro genérica e os detalhes da exceção.</description></item>
        /// </list>
        /// </remarks>
        public static IActionResult Manipulador(Func<IActionResult> func)
        {
            try
            {
                return func();
            }
            catch (ArgumentException ex)
            {
                return new JsonResult(new { sucesso = false, mensagem = ex.Message })
                {
                    StatusCode = 400
                };
            }
            catch (KeyNotFoundException ex)
            {
                return new JsonResult(new { sucesso = false, mensagem = ex.Message })
                {
                    StatusCode = 404
                };
            }
            catch (Exception ex)
            {
                return new JsonResult(new { sucesso = false, mensagem = $"Ocorreu um erro interno no servidor.\n{ex.Message}" })
                {
                    StatusCode = 500
                };
            }
        }
    }
}
