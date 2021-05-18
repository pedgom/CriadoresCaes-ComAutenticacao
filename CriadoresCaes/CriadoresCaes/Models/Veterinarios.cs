using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CriadoresCaes.Models
{
    /// <summary>
    /// dados dos veterinnarios que tratam dos cães
    /// um cão pode ser tratado por muitos veterinários
    /// um Veteri´nário pode, naturalmente, tratar de muitos cães
    /// </summary>
    public class Veterinarios
    {
        public Veterinarios()
        {
            ListaCaesTratadosPeloVeterinario = new HashSet<Caes>();
        }

        /// <summary>
        /// Identificador do Veterinario
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }

        /// <summary>
        /// nome do Veterinário
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Montante cobrado pelo Veterinário numa consulta
        /// </summary>
        public decimal? Honorarios { get; set; }

        /// <summary>
        /// atributo auxiliar para receber o valor dos honorarios do Veterinario
        /// </summary>
        [NotMapped] //este atributo não vai ser adicionado à BD
        [Required]
        [Display(Name ="Honorários")]
        [RegularExpression("[0-9]+[.,]?[0-9]{0,2}")] //formata a texbox para só aceitar valores decimais
       // [RegularExpression("[0-9]+[.,][0-9]{2}?")]   ------> outra alternativa
        public string HonorarioAux { get; set; }

        //***************************************************************************
        //identificar os cães que são tratados pelo Veterinário
        //vamos ignorar a tabela do relacionamento. è como se elea não existisse......
        public ICollection<Caes> ListaCaesTratadosPeloVeterinario { get; set; }




    }
}
