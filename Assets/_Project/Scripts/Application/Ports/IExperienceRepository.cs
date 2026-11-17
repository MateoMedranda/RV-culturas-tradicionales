using RvCulturas.Domain;

namespace RvCulturas.Application.Ports
{
    public interface IExperienceRepository
    {
        ExperienceDefinition Load(string experienceId);
    }
}
