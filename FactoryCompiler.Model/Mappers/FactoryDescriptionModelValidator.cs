using System.Collections.Generic;
using System.Linq;
using FactoryCompiler.Model.Diagnostics;

namespace FactoryCompiler.Model.Mappers;

internal class FactoryDescriptionModelValidator
{
    public ICollection<Diagnostic> Diagnostics { get; set; } = new List<Diagnostic>();

    public void Validate(Dto.FactoryDescription dto)
    {
        if (dto.Regions != null)
        {
            for (var index = 0; index < dto.Regions.Length; index++)
            {
                Validate(dto.Regions[index], index);
            }
        }
    }

    private void Validate(Dto.FactoryDescription.Region region, int index)
    {
        var name = string.IsNullOrWhiteSpace(region.RegionName) ? $"at index {index}" : $"'{region.RegionName}'";
        if (string.IsNullOrWhiteSpace(region.RegionName))
        {
            Diagnostics.Add(Diagnostic.Error($"Region {name} does not specify {nameof(region.RegionName)}."));
        }
        if (region.Groups?.Any() == true)
        {
            Validate(region.Groups, string.Concat("region ", name));
        }
        else
        {
            Diagnostics.Add(Diagnostic.Warning($"Region {name} does not specify any {nameof(region.Groups)}."));
        }
        if (region.Inbound?.Any() == true)
        {
            Validate(region.Inbound, string.Concat("inbound to region ", name));
        }
        if (region.Outbound?.Any() == true)
        {
            Validate(region.Outbound, string.Concat("outbound from region ", name));
        }
    }

    private void Validate(Dto.FactoryDescription.Group[] groups, string parentDescription)
    {
        for (var index = 0; index < groups.Length; index++)
        {
            Validate(groups[index], index, parentDescription);
        }
    }

    private void Validate(Dto.FactoryDescription.Group group, int index, string parentDescription)
    {
        var name = GetEffectiveNameForGroup(group, index, parentDescription);
        if (group.Groups?.Any() == true)
        {
            if (group.Recipe != null)
            {
                Diagnostics.Add(Diagnostic.Error($"Group {name} cannot specify {nameof(group.Recipe)} because it has child groups."));
            }
            else if (group.Count != null)
            {
                Diagnostics.Add(Diagnostic.Error($"Group {name} cannot specify {nameof(group.Count)} because it has no {nameof(group.Recipe)}, and because it has child groups."));
            }
            Validate(group.Groups, string.Concat("group ", name));
            return;
        }
        if (group.Recipe != null)
        {
            if (group.Count == null)
            {
                Diagnostics.Add(Diagnostic.Error($"Group {name} does not specify any {nameof(group.Count)}. '1' will be assumed."));
            }
            return;
        }
        Diagnostics.Add(Diagnostic.Warning($"Group {name} specifies no recipe or {nameof(group.Recipe)} or child groups, and will be ignored."));
    }

    private string GetEffectiveNameForGroup(Dto.FactoryDescription.Group group, int index, string parentDescription)
    {
        if (!string.IsNullOrWhiteSpace(group.GroupName)) return $"'{group.GroupName}' in {parentDescription}";
        if (!string.IsNullOrWhiteSpace(group.Recipe)) return $"producing '{group.Recipe}' in {parentDescription}";
        return $"at index {index} in {parentDescription}";
    }

    private void Validate(Dto.FactoryDescription.Transport[] transports, string parentDescription)
    {
        for (var index = 0; index < transports.Length; index++)
        {
            Validate(transports[index], index, parentDescription);
        }
    }

    private void Validate(Dto.FactoryDescription.Transport transport, int index, string parentDescription)
    {
        var name = string.IsNullOrWhiteSpace(transport.Item) ? $"at index {index} in {parentDescription}" : $"of '{transport.Item}' in {parentDescription}";
        if (string.IsNullOrWhiteSpace(transport.Item))
        {
            Diagnostics.Add(Diagnostic.Error($"Transport {name} does not specify {nameof(transport.Item)}."));
        }
        if (string.IsNullOrWhiteSpace(transport.Network))
        {
            Diagnostics.Add(Diagnostic.Error($"Transport {name} does not specify {nameof(transport.Network)}."));
        }
    }
}
