using Articles.Abstractions.Enums;

namespace Articles.IntregationEvents.Contracts.Articles.Dtos;

public record ActorDto(UserRoleType Role, HashSet<ContributionArea> ContributionAreas, PersonDto Person);
