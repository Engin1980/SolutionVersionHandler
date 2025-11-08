using System;

namespace SolutionVersionHandler.Model
{
  internal sealed record ProjectAndVersion(Project? Project, Version? Version);
}
