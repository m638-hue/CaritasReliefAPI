using CaritasReliefAPI.DBContext;
using CaritasReliefAPI.Schema;
using HotChocolate.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using System.Data.SqlClient;
using System.Net;
using System.Text;

namespace CaritasReliefAPI
{
    public partial class Query
    {
        [Authorize(Roles = new string[] {"admin"})]
        [UseProjection]
        public EstadoRecibosDelDia GetEstadoRecibos (SQLContext context, string date, int? idRecolector = null)
        {
            var recibos = 
                idRecolector == null ? 
                context.Recibos : 
                context.Recibos.Where(r => r.idRecolector == idRecolector);

            return new EstadoRecibosDelDia
            {
                cobradosFallidos = context.Recibos.Count(r => r.cobrado == 0 && r.fecha.Date.ToString() == date),
                cobrados = recibos.Count(r => r.cobrado == 1 && r.fecha.Date.ToString() == date),
                pendiente = recibos.Count(r => r.cobrado == 2 && r.fecha.Date.ToString() == date)
            };
        }

        [Authorize(Roles = new string[] { "admin" })]
        public decimal GetTotalCobrado (SQLContext context, string date, int? idRecolector = null)
        {
            var recibos =
                idRecolector == null ?
                context.Recibos :
                context.Recibos.Where(r => r.idRecolector == idRecolector);

            return recibos
                .Where(r => r.cobrado == 1 && r.fecha.Date.ToString() == date)
                .Sum(r => r.cantidad);
        }

        [Authorize(Roles = new string[] {"admin"})]
        public async Task<HttpStatusCode> TransferirARecolector (SQLContext context, int idRecibo, int idRecolector)
        {
            var recibo = await context.Recibos.FindAsync(idRecibo);

            if (recibo == null) return HttpStatusCode.BadRequest;

            recibo.idRecolector = idRecolector;

            await context.SaveChangesAsync();

            return HttpStatusCode.OK;
        }

        [Authorize(Roles = new string[] {"admin"})]
        public async Task<FileStreamResult> GetResumenRecibos (SQLContext context, string date)
        {
            var sb = new StringBuilder();
            var recibos = await context.Recibos.Where(r => r.fecha.Date.ToString() == date).ToArrayAsync();

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
                FileDownloadName = $"recibos_{date}"
            };
        }
    }
}
