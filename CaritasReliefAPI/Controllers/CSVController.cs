using CaritasReliefAPI.DBContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace CaritasReliefAPI.Controllers
{
    [Route("/resumen")]
    [ApiController]
    public class CSVController : ControllerBase
    {
        private readonly ILogger<CSVController> _logger;

        [Authorize(Roles = "admin")]
        [HttpGet("")]
        public async Task<IActionResult> resumen([FromServices] SQLContext context, string date)
        {
            var sb = new StringBuilder();
            var recibos = await context.Recibos.Where(r => r.fecha.Date.ToString() == date).ToArrayAsync();

            sb.Append("ID Recibo");
            sb.Append(',');
            sb.Append("Fecha");
            sb.Append(',');
            sb.Append("Monto");
            sb.Append(',');
            sb.Append("Estado");
            sb.Append(',');
            sb.Append("Comentarios");
            sb.Append(',');
            sb.Append("Recolector");
            sb.Append(',');
            sb.Append("Donante");
            sb.Append(',');
            sb.AppendLine();

            foreach (var recibo in recibos)
            {
                var recolector = await context.Recolectores.FindAsync(recibo.idRecolector);
                var donante = await context.Donantes.FindAsync(recibo.idDonante);

                sb.Append(recibo.idRecibo);
                sb.Append(',');
                sb.Append(date);
                sb.Append(',');
                sb.Append(recibo.cantidad.ToString());
                sb.Append(',');

                switch (recibo.cobrado)
                {
                    case 0:
                        sb.Append("No cobrado");
                        break;

                    case 1:
                        sb.Append("Cobrado");
                        break;

                    case 2:
                        sb.Append("Pendiente");
                        break;
                }

                sb.Append(',');
                sb.Append(recibo.comentarios);
                sb.Append(',');
                sb.Append(recolector.nombres);
                sb.Append(' ');
                sb.Append(recolector.apellidos);
                sb.Append(',');
                sb.Append(donante.nombres);
                sb.Append(' ');
                sb.Append(donante.apellidos);
                sb.AppendLine();
            }

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);

            writer.Write(sb.ToString());
            writer.Flush();
            stream.Position = 0;

            return new FileStreamResult(stream, "application/octet-stream")
            {
                FileDownloadName = $"recibos_{date}.csv"
            };
        }
    }
}
