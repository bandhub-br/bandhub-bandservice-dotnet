namespace BandHub.BandService.Features.Bands.CreateBand;

public class CreateBandValidator
{
    public List<string> Validate(CreateBandRequest request)
    {
        var errors = new List<string>();
        
        if (string.IsNullOrWhiteSpace(request.Name))
            errors.Add("Name is required.");

        if (string.IsNullOrWhiteSpace(request.Genre))
            errors.Add("Genre is required.");

        if (string.IsNullOrEmpty(request.Description))
            errors.Add("Description is required.");

        if (!string.IsNullOrEmpty(request.Description) && request.Description.Length > 1000)
            errors.Add("Description cannot exceed 1000 characters.");

        if (request.AccountId == Guid.Empty)
            errors.Add("AccountId is required.");

        return errors;
    }
}
