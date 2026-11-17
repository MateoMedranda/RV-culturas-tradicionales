using RvCulturas.Application.Ports;
using RvCulturas.Domain;

namespace RvCulturas.Infrastructure.Content
{
    public sealed class ScriptableObjectExperienceRepository : IExperienceRepository
    {
        private readonly ExperienceDefinitionAsset source;

        public ScriptableObjectExperienceRepository(ExperienceDefinitionAsset source)
        {
            this.source = source;
        }

        public ExperienceDefinition Load(string experienceId)
        {
            if (source == null || source.experienceId != experienceId) return null;
            return source.ToDomain();
        }
    }
}
