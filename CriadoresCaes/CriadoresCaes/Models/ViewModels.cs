using System;
using System.Collections.Generic;

namespace CriadoresCaes.Models
{
    public class FotosCaes 
    {
        /// <summary>
        /// lista de todas as fotografias de todos os c�es
        /// </summary>
        public ICollection<Fotografias> ListaFotografias { get; set; }

        /// <summary>
        /// lista dos IDs dos c�es que pertencem � pessoa autenticada
        /// </summary>
        public ICollection<int> ListaCaes { get; set; }
    }

    public class ErrorViewModel
    {
        public string RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }

}
