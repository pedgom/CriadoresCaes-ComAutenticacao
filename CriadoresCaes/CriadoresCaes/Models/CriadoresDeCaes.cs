using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CriadoresCaes.Models
{
    public class CriadoresDeCaes
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Data de Compra
        /// </summary>
        public DateTime DataCompra { get; set; }

        //*************************************************************

        /// <summary>
        /// FK para o Cão do Criador
        /// </summary>
        [ForeignKey(nameof(Cao))]
        public int CaoFK { get; set; }
        public Caes Cao { get; set; }


        /// <summary>
        /// FK para o Criador do Cão
        /// </summary>
        [ForeignKey(nameof(Criador))]  // [ForeignKey("Criador")]
        public int CriadorFK { get; set; }
        public Criadores Criador { get; set; }

    }
}
