using OpenRA.Traits;

namespace OpenRA.Mods.RA2.Mechanics.SlaveMiner.Activities
{
	[RequireExplicitImplementation]
	public interface INotifySlaveEntering
	{
		void OnSlaveEntering(Actor self, Actor slave);
		void OnSlaveEntered(Actor self, Actor slave);
	}
}
