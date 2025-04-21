namespace LabTestBrowser.UseCases.SpecimenCollectionCenters.List;

public record ListSpecimenCollectionCentersQuery(int? Skip, int? Take) : IQuery<Result<IEnumerable<SpecimenCollectionCenterDto>>>;