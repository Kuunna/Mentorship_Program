using QuizChallenge.Repositories;
using QuizChallenge.Models;

namespace QuizChallenge.Services
{
    public class TypeService
    {
        private readonly TypeRepository _typeRepository;

        public TypeService(TypeRepository typeRepository)
        {
            _typeRepository = typeRepository;
        }

        public List<Models.Type> GetAllTypes() => _typeRepository.GetAllTypes();

        public void AddType(Models.Type type) => _typeRepository.AddType(type);

        public void UpdateType(Models.Type type) => _typeRepository.UpdateType(type);

        public void DeleteType(int typeId) => _typeRepository.DeleteType(typeId);
    }

}
