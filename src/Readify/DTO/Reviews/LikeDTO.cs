using Readify.DTO.Users;
using System;

namespace Readify.DTO.Reviews
{
    public class LikeDTO
    {
        /// <summary>
        /// Id лайка
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Id автора лайка
        /// </summary>
        public int? IdAuthor { get; set; }

        /// <summary>
        /// Id отзыва, на который поставлен лайк
        /// </summary>
        public int IdReview { get; set; }

        /// <summary>
        /// 0 - дизлайк, 1 - лайк
        /// </summary>
        public byte ReactionType { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
