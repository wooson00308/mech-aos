// <auto-generated>
// This code was auto-generated by a tool, every time
// the tool executes this code will be reset.
//
// If you need to extend the classes generated to add
// fields or methods to them, please create partial
// declarations in another file.
// </auto-generated>
#pragma warning disable 0109
#pragma warning disable 1591


namespace Quantum {
  using Photon.Deterministic;
  using Quantum;
  using Quantum.Core;
  using Quantum.Collections;
  using Quantum.Inspector;
  using Quantum.Physics2D;
  using Quantum.Physics3D;
  using Byte = System.Byte;
  using SByte = System.SByte;
  using Int16 = System.Int16;
  using UInt16 = System.UInt16;
  using Int32 = System.Int32;
  using UInt32 = System.UInt32;
  using Int64 = System.Int64;
  using UInt64 = System.UInt64;
  using Boolean = System.Boolean;
  using String = System.String;
  using Object = System.Object;
  using FlagsAttribute = System.FlagsAttribute;
  using SerializableAttribute = System.SerializableAttribute;
  using MethodImplAttribute = System.Runtime.CompilerServices.MethodImplAttribute;
  using MethodImplOptions = System.Runtime.CompilerServices.MethodImplOptions;
  using FieldOffsetAttribute = System.Runtime.InteropServices.FieldOffsetAttribute;
  using StructLayoutAttribute = System.Runtime.InteropServices.StructLayoutAttribute;
  using LayoutKind = System.Runtime.InteropServices.LayoutKind;
  #if QUANTUM_UNITY //;
  using TooltipAttribute = UnityEngine.TooltipAttribute;
  using HeaderAttribute = UnityEngine.HeaderAttribute;
  using SpaceAttribute = UnityEngine.SpaceAttribute;
  using RangeAttribute = UnityEngine.RangeAttribute;
  using HideInInspectorAttribute = UnityEngine.HideInInspector;
  using PreserveAttribute = UnityEngine.Scripting.PreserveAttribute;
  using FormerlySerializedAsAttribute = UnityEngine.Serialization.FormerlySerializedAsAttribute;
  using MovedFromAttribute = UnityEngine.Scripting.APIUpdating.MovedFromAttribute;
  using CreateAssetMenu = UnityEngine.CreateAssetMenuAttribute;
  using RuntimeInitializeOnLoadMethodAttribute = UnityEngine.RuntimeInitializeOnLoadMethodAttribute;
  #endif //;
  
  public unsafe partial class Frame {
    public unsafe partial struct FrameEvents {
      static partial void GetEventTypeCountCodeGen(ref Int32 eventCount) {
        eventCount = 37;
      }
      static partial void GetParentEventIDCodeGen(Int32 eventID, ref Int32 parentEventID) {
        switch (eventID) {
          case EventOnMechanicTakeDamage.ID: parentEventID = EventMechanicEvent.ID; return;
          case EventOnMechanicDeath.ID: parentEventID = EventMechanicEvent.ID; return;
          case EventOnMechanicCreated.ID: parentEventID = EventMechanicEvent.ID; return;
          case EventOnMechanicRespawn.ID: parentEventID = EventMechanicEvent.ID; return;
          case EventOnMechanicChangeWeapon.ID: parentEventID = EventMechanicEvent.ID; return;
          case EventOnMechanicDashed.ID: parentEventID = EventMechanicEvent.ID; return;
          case EventOnMechanicOrbitalSupport.ID: parentEventID = EventMechanicEvent.ID; return;
          case EventOnMechanicOrbitalSupportEnd.ID: parentEventID = EventMechanicEvent.ID; return;
          case EventOnNexusTakeDamage.ID: parentEventID = EventNexusEvent.ID; return;
          case EventOnNexusDestroy.ID: parentEventID = EventNexusEvent.ID; return;
          default: break;
        }
      }
      static partial void GetEventTypeCodeGen(Int32 eventID, ref System.Type result) {
        switch (eventID) {
          case EventOnBulletDestroyed.ID: result = typeof(EventOnBulletDestroyed); return;
          case EventTowerActivate.ID: result = typeof(EventTowerActivate); return;
          case EventTowerAttack.ID: result = typeof(EventTowerAttack); return;
          case EventMechanicEvent.ID: result = typeof(EventMechanicEvent); return;
          case EventOnMechanicTakeDamage.ID: result = typeof(EventOnMechanicTakeDamage); return;
          case EventOnMechanicDeath.ID: result = typeof(EventOnMechanicDeath); return;
          case EventOnMechanicCreated.ID: result = typeof(EventOnMechanicCreated); return;
          case EventOnMechanicRespawn.ID: result = typeof(EventOnMechanicRespawn); return;
          case EventOnMechanicChangeWeapon.ID: result = typeof(EventOnMechanicChangeWeapon); return;
          case EventOnMechanicDashed.ID: result = typeof(EventOnMechanicDashed); return;
          case EventOnMechanicOrbitalSupport.ID: result = typeof(EventOnMechanicOrbitalSupport); return;
          case EventOnMechanicOrbitalSupportEnd.ID: result = typeof(EventOnMechanicOrbitalSupportEnd); return;
          case EventOnGameEnded.ID: result = typeof(EventOnGameEnded); return;
          case EventOnGameStateChanged.ID: result = typeof(EventOnGameStateChanged); return;
          case EventOnCountdownStart.ID: result = typeof(EventOnCountdownStart); return;
          case EventOnGameStart.ID: result = typeof(EventOnGameStart); return;
          case EventOnPlayerDeath.ID: result = typeof(EventOnPlayerDeath); return;
          case EventOnPlayerNexusDestoryed.ID: result = typeof(EventOnPlayerNexusDestoryed); return;
          case EventOnPlayerKilled.ID: result = typeof(EventOnPlayerKilled); return;
          case EventOnPlayerTeamWin.ID: result = typeof(EventOnPlayerTeamWin); return;
          case EventShutdown.ID: result = typeof(EventShutdown); return;
          case EventGameStateChanged.ID: result = typeof(EventGameStateChanged); return;
          case EventMovement.ID: result = typeof(EventMovement); return;
          case EventWeaponFire.ID: result = typeof(EventWeaponFire); return;
          case EventUseSkill.ID: result = typeof(EventUseSkill); return;
          case EventFix.ID: result = typeof(EventFix); return;
          case EventNexusEvent.ID: result = typeof(EventNexusEvent); return;
          case EventOnNexusTakeDamage.ID: result = typeof(EventOnNexusTakeDamage); return;
          case EventOnEnableFix.ID: result = typeof(EventOnEnableFix); return;
          case EventOnNexusDestroy.ID: result = typeof(EventOnNexusDestroy); return;
          case EventOnTeamNexusDestroy.ID: result = typeof(EventOnTeamNexusDestroy); return;
          case EventPlayerLeft.ID: result = typeof(EventPlayerLeft); return;
          case EventOnChangeWeapon.ID: result = typeof(EventOnChangeWeapon); return;
          case EventOnTrapDestroyed.ID: result = typeof(EventOnTrapDestroyed); return;
          case EventOnWeaponShoot.ID: result = typeof(EventOnWeaponShoot); return;
          case EventDummyEvent.ID: result = typeof(EventDummyEvent); return;
          default: break;
        }
      }
      public EventOnBulletDestroyed OnBulletDestroyed(Int32 BulletRefHashCode, EntityRef Mechanic, FPVector3 BulletPosition, FPVector3 BulletDirection, AssetRef<BulletData> BulletData) {
        var ev = _f.Context.AcquireEvent<EventOnBulletDestroyed>(EventOnBulletDestroyed.ID);
        ev.BulletRefHashCode = BulletRefHashCode;
        ev.Mechanic = Mechanic;
        ev.BulletPosition = BulletPosition;
        ev.BulletDirection = BulletDirection;
        ev.BulletData = BulletData;
        _f.AddEvent(ev);
        return ev;
      }
      public EventTowerActivate TowerActivate(Team team, QBoolean isActive) {
        var ev = _f.Context.AcquireEvent<EventTowerActivate>(EventTowerActivate.ID);
        ev.team = team;
        ev.isActive = isActive;
        _f.AddEvent(ev);
        return ev;
      }
      public EventTowerAttack TowerAttack(Team Team, EntityRef bullet, EntityRef nexus, FP FirstDelayTime, FP damage) {
        var ev = _f.Context.AcquireEvent<EventTowerAttack>(EventTowerAttack.ID);
        ev.Team = Team;
        ev.bullet = bullet;
        ev.nexus = nexus;
        ev.FirstDelayTime = FirstDelayTime;
        ev.damage = damage;
        _f.AddEvent(ev);
        return ev;
      }
      public EventOnMechanicTakeDamage OnMechanicTakeDamage(EntityRef Mechanic, FP Damage, EntityRef Source) {
        var ev = _f.Context.AcquireEvent<EventOnMechanicTakeDamage>(EventOnMechanicTakeDamage.ID);
        ev.Mechanic = Mechanic;
        ev.Damage = Damage;
        ev.Source = Source;
        _f.AddEvent(ev);
        return ev;
      }
      public EventOnMechanicDeath OnMechanicDeath(EntityRef Mechanic, EntityRef Killer) {
        if (_f.IsPredicted) return null;
        var ev = _f.Context.AcquireEvent<EventOnMechanicDeath>(EventOnMechanicDeath.ID);
        ev.Mechanic = Mechanic;
        ev.Killer = Killer;
        _f.AddEvent(ev);
        return ev;
      }
      public EventOnMechanicCreated OnMechanicCreated(EntityRef Mechanic) {
        var ev = _f.Context.AcquireEvent<EventOnMechanicCreated>(EventOnMechanicCreated.ID);
        ev.Mechanic = Mechanic;
        _f.AddEvent(ev);
        return ev;
      }
      public EventOnMechanicRespawn OnMechanicRespawn(EntityRef Mechanic) {
        var ev = _f.Context.AcquireEvent<EventOnMechanicRespawn>(EventOnMechanicRespawn.ID);
        ev.Mechanic = Mechanic;
        _f.AddEvent(ev);
        return ev;
      }
      public EventOnMechanicChangeWeapon OnMechanicChangeWeapon(EntityRef Mechanic) {
        var ev = _f.Context.AcquireEvent<EventOnMechanicChangeWeapon>(EventOnMechanicChangeWeapon.ID);
        ev.Mechanic = Mechanic;
        _f.AddEvent(ev);
        return ev;
      }
      public EventOnMechanicDashed OnMechanicDashed(EntityRef Mechanic) {
        var ev = _f.Context.AcquireEvent<EventOnMechanicDashed>(EventOnMechanicDashed.ID);
        ev.Mechanic = Mechanic;
        _f.AddEvent(ev);
        return ev;
      }
      public EventOnMechanicOrbitalSupport OnMechanicOrbitalSupport(EntityRef Mechanic) {
        var ev = _f.Context.AcquireEvent<EventOnMechanicOrbitalSupport>(EventOnMechanicOrbitalSupport.ID);
        ev.Mechanic = Mechanic;
        _f.AddEvent(ev);
        return ev;
      }
      public EventOnMechanicOrbitalSupportEnd OnMechanicOrbitalSupportEnd(EntityRef Mechanic) {
        var ev = _f.Context.AcquireEvent<EventOnMechanicOrbitalSupportEnd>(EventOnMechanicOrbitalSupportEnd.ID);
        ev.Mechanic = Mechanic;
        _f.AddEvent(ev);
        return ev;
      }
      public EventOnGameEnded OnGameEnded() {
        if (_f.IsPredicted) return null;
        var ev = _f.Context.AcquireEvent<EventOnGameEnded>(EventOnGameEnded.ID);
        _f.AddEvent(ev);
        return ev;
      }
      public EventOnGameStateChanged OnGameStateChanged(GameState state) {
        if (_f.IsPredicted) return null;
        var ev = _f.Context.AcquireEvent<EventOnGameStateChanged>(EventOnGameStateChanged.ID);
        ev.state = state;
        _f.AddEvent(ev);
        return ev;
      }
      public EventOnCountdownStart OnCountdownStart(FP Time) {
        if (_f.IsPredicted) return null;
        var ev = _f.Context.AcquireEvent<EventOnCountdownStart>(EventOnCountdownStart.ID);
        ev.Time = Time;
        _f.AddEvent(ev);
        return ev;
      }
      public EventOnGameStart OnGameStart() {
        if (_f.IsPredicted) return null;
        var ev = _f.Context.AcquireEvent<EventOnGameStart>(EventOnGameStart.ID);
        _f.AddEvent(ev);
        return ev;
      }
      public EventOnPlayerDeath OnPlayerDeath(EntityRef entityRef) {
        if (_f.IsPredicted) return null;
        var ev = _f.Context.AcquireEvent<EventOnPlayerDeath>(EventOnPlayerDeath.ID);
        ev.entityRef = entityRef;
        _f.AddEvent(ev);
        return ev;
      }
      public EventOnPlayerNexusDestoryed OnPlayerNexusDestoryed(Team team) {
        if (_f.IsPredicted) return null;
        var ev = _f.Context.AcquireEvent<EventOnPlayerNexusDestoryed>(EventOnPlayerNexusDestoryed.ID);
        ev.team = team;
        _f.AddEvent(ev);
        return ev;
      }
      public EventOnPlayerKilled OnPlayerKilled(EntityRef target, EntityRef killer) {
        if (_f.IsPredicted) return null;
        var ev = _f.Context.AcquireEvent<EventOnPlayerKilled>(EventOnPlayerKilled.ID);
        ev.target = target;
        ev.killer = killer;
        _f.AddEvent(ev);
        return ev;
      }
      public EventOnPlayerTeamWin OnPlayerTeamWin(Team team) {
        if (_f.IsPredicted) return null;
        var ev = _f.Context.AcquireEvent<EventOnPlayerTeamWin>(EventOnPlayerTeamWin.ID);
        ev.team = team;
        _f.AddEvent(ev);
        return ev;
      }
      public EventShutdown Shutdown() {
        var ev = _f.Context.AcquireEvent<EventShutdown>(EventShutdown.ID);
        _f.AddEvent(ev);
        return ev;
      }
      public EventGameStateChanged GameStateChanged(GameState NewState, GameState OldState) {
        if (_f.IsPredicted) return null;
        var ev = _f.Context.AcquireEvent<EventGameStateChanged>(EventGameStateChanged.ID);
        ev.NewState = NewState;
        ev.OldState = OldState;
        _f.AddEvent(ev);
        return ev;
      }
      public EventMovement Movement(EntityRef Owner, FP Velocity) {
        var ev = _f.Context.AcquireEvent<EventMovement>(EventMovement.ID);
        ev.Owner = Owner;
        ev.Velocity = Velocity;
        _f.AddEvent(ev);
        return ev;
      }
      public EventWeaponFire WeaponFire(EntityRef Owner, AssetRef<WeaponData> WeaponData) {
        var ev = _f.Context.AcquireEvent<EventWeaponFire>(EventWeaponFire.ID);
        ev.Owner = Owner;
        ev.WeaponData = WeaponData;
        _f.AddEvent(ev);
        return ev;
      }
      public EventUseSkill UseSkill(EntityRef Owner, Skill skill, Weapon weapon, FP index) {
        var ev = _f.Context.AcquireEvent<EventUseSkill>(EventUseSkill.ID);
        ev.Owner = Owner;
        ev.skill = skill;
        ev.weapon = weapon;
        ev.index = index;
        _f.AddEvent(ev);
        return ev;
      }
      public EventFix Fix() {
        var ev = _f.Context.AcquireEvent<EventFix>(EventFix.ID);
        _f.AddEvent(ev);
        return ev;
      }
      public EventOnNexusTakeDamage OnNexusTakeDamage(EntityRef Nexus, FP Damage, EntityRef Source) {
        var ev = _f.Context.AcquireEvent<EventOnNexusTakeDamage>(EventOnNexusTakeDamage.ID);
        ev.Nexus = Nexus;
        ev.Damage = Damage;
        ev.Source = Source;
        _f.AddEvent(ev);
        return ev;
      }
      public EventOnEnableFix OnEnableFix(EntityRef mechanic, EntityRef nexusIndentifier, QBoolean isEnabled) {
        var ev = _f.Context.AcquireEvent<EventOnEnableFix>(EventOnEnableFix.ID);
        ev.mechanic = mechanic;
        ev.nexusIndentifier = nexusIndentifier;
        ev.isEnabled = isEnabled;
        _f.AddEvent(ev);
        return ev;
      }
      public EventOnNexusDestroy OnNexusDestroy(EntityRef Nexus, EntityRef Killer) {
        if (_f.IsPredicted) return null;
        var ev = _f.Context.AcquireEvent<EventOnNexusDestroy>(EventOnNexusDestroy.ID);
        ev.Nexus = Nexus;
        ev.Killer = Killer;
        _f.AddEvent(ev);
        return ev;
      }
      public EventOnTeamNexusDestroy OnTeamNexusDestroy(Team Team) {
        if (_f.IsPredicted) return null;
        var ev = _f.Context.AcquireEvent<EventOnTeamNexusDestroy>(EventOnTeamNexusDestroy.ID);
        ev.Team = Team;
        _f.AddEvent(ev);
        return ev;
      }
      public EventPlayerLeft PlayerLeft(PlayerRef Player) {
        var ev = _f.Context.AcquireEvent<EventPlayerLeft>(EventPlayerLeft.ID);
        ev.Player = Player;
        _f.AddEvent(ev);
        return ev;
      }
      public EventOnChangeWeapon OnChangeWeapon(EntityRef Mechanic, Weapon weapon) {
        var ev = _f.Context.AcquireEvent<EventOnChangeWeapon>(EventOnChangeWeapon.ID);
        ev.Mechanic = Mechanic;
        ev.weapon = weapon;
        _f.AddEvent(ev);
        return ev;
      }
      public EventOnTrapDestroyed OnTrapDestroyed(Int32 TrapRefHashCode, EntityRef Mechanic, FPVector3 TrapPosition, AssetRef<TrapData> TrapData) {
        var ev = _f.Context.AcquireEvent<EventOnTrapDestroyed>(EventOnTrapDestroyed.ID);
        ev.TrapRefHashCode = TrapRefHashCode;
        ev.Mechanic = Mechanic;
        ev.TrapPosition = TrapPosition;
        ev.TrapData = TrapData;
        _f.AddEvent(ev);
        return ev;
      }
      public EventOnWeaponShoot OnWeaponShoot(EntityRef Mechanic) {
        var ev = _f.Context.AcquireEvent<EventOnWeaponShoot>(EventOnWeaponShoot.ID);
        ev.Mechanic = Mechanic;
        _f.AddEvent(ev);
        return ev;
      }
      public EventDummyEvent DummyEvent() {
        var ev = _f.Context.AcquireEvent<EventDummyEvent>(EventDummyEvent.ID);
        _f.AddEvent(ev);
        return ev;
      }
    }
  }
  public unsafe partial class EventOnBulletDestroyed : EventBase {
    public new const Int32 ID = 1;
    public Int32 BulletRefHashCode;
    public EntityRef Mechanic;
    public FPVector3 BulletPosition;
    public FPVector3 BulletDirection;
    public AssetRef<BulletData> BulletData;
    protected EventOnBulletDestroyed(Int32 id, EventFlags flags) : 
        base(id, flags) {
    }
    public EventOnBulletDestroyed() : 
        base(1, EventFlags.Server|EventFlags.Client) {
    }
    public new QuantumGame Game {
      get {
        return (QuantumGame)base.Game;
      }
      set {
        base.Game = value;
      }
    }
    public override Int32 GetHashCode() {
      unchecked {
        var hash = 41;
        hash = hash * 31 + BulletRefHashCode.GetHashCode();
        hash = hash * 31 + Mechanic.GetHashCode();
        hash = hash * 31 + BulletPosition.GetHashCode();
        hash = hash * 31 + BulletDirection.GetHashCode();
        hash = hash * 31 + BulletData.GetHashCode();
        return hash;
      }
    }
  }
  public unsafe partial class EventTowerActivate : EventBase {
    public new const Int32 ID = 2;
    public Team team;
    public QBoolean isActive;
    protected EventTowerActivate(Int32 id, EventFlags flags) : 
        base(id, flags) {
    }
    public EventTowerActivate() : 
        base(2, EventFlags.Server|EventFlags.Client) {
    }
    public new QuantumGame Game {
      get {
        return (QuantumGame)base.Game;
      }
      set {
        base.Game = value;
      }
    }
    public override Int32 GetHashCode() {
      unchecked {
        var hash = 43;
        hash = hash * 31 + team.GetHashCode();
        hash = hash * 31 + isActive.GetHashCode();
        return hash;
      }
    }
  }
  public unsafe partial class EventTowerAttack : EventBase {
    public new const Int32 ID = 3;
    public Team Team;
    public EntityRef bullet;
    public EntityRef nexus;
    public FP FirstDelayTime;
    public FP damage;
    protected EventTowerAttack(Int32 id, EventFlags flags) : 
        base(id, flags) {
    }
    public EventTowerAttack() : 
        base(3, EventFlags.Server|EventFlags.Client) {
    }
    public new QuantumGame Game {
      get {
        return (QuantumGame)base.Game;
      }
      set {
        base.Game = value;
      }
    }
    public override Int32 GetHashCode() {
      unchecked {
        var hash = 47;
        hash = hash * 31 + Team.GetHashCode();
        hash = hash * 31 + bullet.GetHashCode();
        hash = hash * 31 + nexus.GetHashCode();
        hash = hash * 31 + FirstDelayTime.GetHashCode();
        hash = hash * 31 + damage.GetHashCode();
        return hash;
      }
    }
  }
  public abstract unsafe partial class EventMechanicEvent : EventBase {
    public new const Int32 ID = 4;
    public EntityRef Mechanic;
    protected EventMechanicEvent(Int32 id, EventFlags flags) : 
        base(id, flags) {
    }
    public new QuantumGame Game {
      get {
        return (QuantumGame)base.Game;
      }
      set {
        base.Game = value;
      }
    }
    public override Int32 GetHashCode() {
      unchecked {
        var hash = 53;
        hash = hash * 31 + Mechanic.GetHashCode();
        return hash;
      }
    }
  }
  public unsafe partial class EventOnMechanicTakeDamage : EventMechanicEvent {
    public new const Int32 ID = 5;
    public FP Damage;
    public EntityRef Source;
    protected EventOnMechanicTakeDamage(Int32 id, EventFlags flags) : 
        base(id, flags) {
    }
    public EventOnMechanicTakeDamage() : 
        base(5, EventFlags.Server|EventFlags.Client) {
    }
    public override Int32 GetHashCode() {
      unchecked {
        var hash = 59;
        hash = hash * 31 + Mechanic.GetHashCode();
        hash = hash * 31 + Damage.GetHashCode();
        hash = hash * 31 + Source.GetHashCode();
        return hash;
      }
    }
  }
  public unsafe partial class EventOnMechanicDeath : EventMechanicEvent {
    public new const Int32 ID = 6;
    public EntityRef Killer;
    protected EventOnMechanicDeath(Int32 id, EventFlags flags) : 
        base(id, flags) {
    }
    public EventOnMechanicDeath() : 
        base(6, EventFlags.Server|EventFlags.Client|EventFlags.Synced) {
    }
    public override Int32 GetHashCode() {
      unchecked {
        var hash = 61;
        hash = hash * 31 + Mechanic.GetHashCode();
        hash = hash * 31 + Killer.GetHashCode();
        return hash;
      }
    }
  }
  public unsafe partial class EventOnMechanicCreated : EventMechanicEvent {
    public new const Int32 ID = 7;
    protected EventOnMechanicCreated(Int32 id, EventFlags flags) : 
        base(id, flags) {
    }
    public EventOnMechanicCreated() : 
        base(7, EventFlags.Server|EventFlags.Client) {
    }
    public override Int32 GetHashCode() {
      unchecked {
        var hash = 67;
        hash = hash * 31 + Mechanic.GetHashCode();
        return hash;
      }
    }
  }
  public unsafe partial class EventOnMechanicRespawn : EventMechanicEvent {
    public new const Int32 ID = 8;
    protected EventOnMechanicRespawn(Int32 id, EventFlags flags) : 
        base(id, flags) {
    }
    public EventOnMechanicRespawn() : 
        base(8, EventFlags.Server|EventFlags.Client) {
    }
    public override Int32 GetHashCode() {
      unchecked {
        var hash = 71;
        hash = hash * 31 + Mechanic.GetHashCode();
        return hash;
      }
    }
  }
  public unsafe partial class EventOnMechanicChangeWeapon : EventMechanicEvent {
    public new const Int32 ID = 9;
    protected EventOnMechanicChangeWeapon(Int32 id, EventFlags flags) : 
        base(id, flags) {
    }
    public EventOnMechanicChangeWeapon() : 
        base(9, EventFlags.Server|EventFlags.Client) {
    }
    public override Int32 GetHashCode() {
      unchecked {
        var hash = 73;
        hash = hash * 31 + Mechanic.GetHashCode();
        return hash;
      }
    }
  }
  public unsafe partial class EventOnMechanicDashed : EventMechanicEvent {
    public new const Int32 ID = 10;
    protected EventOnMechanicDashed(Int32 id, EventFlags flags) : 
        base(id, flags) {
    }
    public EventOnMechanicDashed() : 
        base(10, EventFlags.Server|EventFlags.Client) {
    }
    public override Int32 GetHashCode() {
      unchecked {
        var hash = 79;
        hash = hash * 31 + Mechanic.GetHashCode();
        return hash;
      }
    }
  }
  public unsafe partial class EventOnMechanicOrbitalSupport : EventMechanicEvent {
    public new const Int32 ID = 11;
    protected EventOnMechanicOrbitalSupport(Int32 id, EventFlags flags) : 
        base(id, flags) {
    }
    public EventOnMechanicOrbitalSupport() : 
        base(11, EventFlags.Server|EventFlags.Client) {
    }
    public override Int32 GetHashCode() {
      unchecked {
        var hash = 83;
        hash = hash * 31 + Mechanic.GetHashCode();
        return hash;
      }
    }
  }
  public unsafe partial class EventOnMechanicOrbitalSupportEnd : EventMechanicEvent {
    public new const Int32 ID = 12;
    protected EventOnMechanicOrbitalSupportEnd(Int32 id, EventFlags flags) : 
        base(id, flags) {
    }
    public EventOnMechanicOrbitalSupportEnd() : 
        base(12, EventFlags.Server|EventFlags.Client) {
    }
    public override Int32 GetHashCode() {
      unchecked {
        var hash = 89;
        hash = hash * 31 + Mechanic.GetHashCode();
        return hash;
      }
    }
  }
  public unsafe partial class EventOnGameEnded : EventBase {
    public new const Int32 ID = 13;
    protected EventOnGameEnded(Int32 id, EventFlags flags) : 
        base(id, flags) {
    }
    public EventOnGameEnded() : 
        base(13, EventFlags.Server|EventFlags.Client|EventFlags.Synced) {
    }
    public new QuantumGame Game {
      get {
        return (QuantumGame)base.Game;
      }
      set {
        base.Game = value;
      }
    }
    public override Int32 GetHashCode() {
      unchecked {
        var hash = 97;
        return hash;
      }
    }
  }
  public unsafe partial class EventOnGameStateChanged : EventBase {
    public new const Int32 ID = 14;
    public GameState state;
    protected EventOnGameStateChanged(Int32 id, EventFlags flags) : 
        base(id, flags) {
    }
    public EventOnGameStateChanged() : 
        base(14, EventFlags.Server|EventFlags.Client|EventFlags.Synced) {
    }
    public new QuantumGame Game {
      get {
        return (QuantumGame)base.Game;
      }
      set {
        base.Game = value;
      }
    }
    public override Int32 GetHashCode() {
      unchecked {
        var hash = 101;
        hash = hash * 31 + state.GetHashCode();
        return hash;
      }
    }
  }
  public unsafe partial class EventOnCountdownStart : EventBase {
    public new const Int32 ID = 15;
    public FP Time;
    protected EventOnCountdownStart(Int32 id, EventFlags flags) : 
        base(id, flags) {
    }
    public EventOnCountdownStart() : 
        base(15, EventFlags.Server|EventFlags.Client|EventFlags.Synced) {
    }
    public new QuantumGame Game {
      get {
        return (QuantumGame)base.Game;
      }
      set {
        base.Game = value;
      }
    }
    public override Int32 GetHashCode() {
      unchecked {
        var hash = 103;
        hash = hash * 31 + Time.GetHashCode();
        return hash;
      }
    }
  }
  public unsafe partial class EventOnGameStart : EventBase {
    public new const Int32 ID = 16;
    protected EventOnGameStart(Int32 id, EventFlags flags) : 
        base(id, flags) {
    }
    public EventOnGameStart() : 
        base(16, EventFlags.Server|EventFlags.Client|EventFlags.Synced) {
    }
    public new QuantumGame Game {
      get {
        return (QuantumGame)base.Game;
      }
      set {
        base.Game = value;
      }
    }
    public override Int32 GetHashCode() {
      unchecked {
        var hash = 107;
        return hash;
      }
    }
  }
  public unsafe partial class EventOnPlayerDeath : EventBase {
    public new const Int32 ID = 17;
    public EntityRef entityRef;
    protected EventOnPlayerDeath(Int32 id, EventFlags flags) : 
        base(id, flags) {
    }
    public EventOnPlayerDeath() : 
        base(17, EventFlags.Server|EventFlags.Client|EventFlags.Synced) {
    }
    public new QuantumGame Game {
      get {
        return (QuantumGame)base.Game;
      }
      set {
        base.Game = value;
      }
    }
    public override Int32 GetHashCode() {
      unchecked {
        var hash = 109;
        hash = hash * 31 + entityRef.GetHashCode();
        return hash;
      }
    }
  }
  public unsafe partial class EventOnPlayerNexusDestoryed : EventBase {
    public new const Int32 ID = 18;
    public Team team;
    protected EventOnPlayerNexusDestoryed(Int32 id, EventFlags flags) : 
        base(id, flags) {
    }
    public EventOnPlayerNexusDestoryed() : 
        base(18, EventFlags.Server|EventFlags.Client|EventFlags.Synced) {
    }
    public new QuantumGame Game {
      get {
        return (QuantumGame)base.Game;
      }
      set {
        base.Game = value;
      }
    }
    public override Int32 GetHashCode() {
      unchecked {
        var hash = 113;
        hash = hash * 31 + team.GetHashCode();
        return hash;
      }
    }
  }
  public unsafe partial class EventOnPlayerKilled : EventBase {
    public new const Int32 ID = 19;
    public EntityRef target;
    public EntityRef killer;
    protected EventOnPlayerKilled(Int32 id, EventFlags flags) : 
        base(id, flags) {
    }
    public EventOnPlayerKilled() : 
        base(19, EventFlags.Server|EventFlags.Client|EventFlags.Synced) {
    }
    public new QuantumGame Game {
      get {
        return (QuantumGame)base.Game;
      }
      set {
        base.Game = value;
      }
    }
    public override Int32 GetHashCode() {
      unchecked {
        var hash = 127;
        hash = hash * 31 + target.GetHashCode();
        hash = hash * 31 + killer.GetHashCode();
        return hash;
      }
    }
  }
  public unsafe partial class EventOnPlayerTeamWin : EventBase {
    public new const Int32 ID = 20;
    public Team team;
    protected EventOnPlayerTeamWin(Int32 id, EventFlags flags) : 
        base(id, flags) {
    }
    public EventOnPlayerTeamWin() : 
        base(20, EventFlags.Server|EventFlags.Client|EventFlags.Synced) {
    }
    public new QuantumGame Game {
      get {
        return (QuantumGame)base.Game;
      }
      set {
        base.Game = value;
      }
    }
    public override Int32 GetHashCode() {
      unchecked {
        var hash = 131;
        hash = hash * 31 + team.GetHashCode();
        return hash;
      }
    }
  }
  public unsafe partial class EventShutdown : EventBase {
    public new const Int32 ID = 21;
    protected EventShutdown(Int32 id, EventFlags flags) : 
        base(id, flags) {
    }
    public EventShutdown() : 
        base(21, EventFlags.Server|EventFlags.Client) {
    }
    public new QuantumGame Game {
      get {
        return (QuantumGame)base.Game;
      }
      set {
        base.Game = value;
      }
    }
    public override Int32 GetHashCode() {
      unchecked {
        var hash = 137;
        return hash;
      }
    }
  }
  public unsafe partial class EventGameStateChanged : EventBase {
    public new const Int32 ID = 22;
    public GameState NewState;
    public GameState OldState;
    protected EventGameStateChanged(Int32 id, EventFlags flags) : 
        base(id, flags) {
    }
    public EventGameStateChanged() : 
        base(22, EventFlags.Server|EventFlags.Client|EventFlags.Synced) {
    }
    public new QuantumGame Game {
      get {
        return (QuantumGame)base.Game;
      }
      set {
        base.Game = value;
      }
    }
    public override Int32 GetHashCode() {
      unchecked {
        var hash = 139;
        hash = hash * 31 + NewState.GetHashCode();
        hash = hash * 31 + OldState.GetHashCode();
        return hash;
      }
    }
  }
  public unsafe partial class EventMovement : EventBase {
    public new const Int32 ID = 23;
    public EntityRef Owner;
    public FP Velocity;
    protected EventMovement(Int32 id, EventFlags flags) : 
        base(id, flags) {
    }
    public EventMovement() : 
        base(23, EventFlags.Server|EventFlags.Client) {
    }
    public new QuantumGame Game {
      get {
        return (QuantumGame)base.Game;
      }
      set {
        base.Game = value;
      }
    }
    public override Int32 GetHashCode() {
      unchecked {
        var hash = 149;
        hash = hash * 31 + Owner.GetHashCode();
        hash = hash * 31 + Velocity.GetHashCode();
        return hash;
      }
    }
  }
  public unsafe partial class EventWeaponFire : EventBase {
    public new const Int32 ID = 24;
    public EntityRef Owner;
    public AssetRef<WeaponData> WeaponData;
    protected EventWeaponFire(Int32 id, EventFlags flags) : 
        base(id, flags) {
    }
    public EventWeaponFire() : 
        base(24, EventFlags.Server|EventFlags.Client) {
    }
    public new QuantumGame Game {
      get {
        return (QuantumGame)base.Game;
      }
      set {
        base.Game = value;
      }
    }
    public override Int32 GetHashCode() {
      unchecked {
        var hash = 151;
        hash = hash * 31 + Owner.GetHashCode();
        hash = hash * 31 + WeaponData.GetHashCode();
        return hash;
      }
    }
  }
  public unsafe partial class EventUseSkill : EventBase {
    public new const Int32 ID = 25;
    public EntityRef Owner;
    public Skill skill;
    public Weapon weapon;
    public FP index;
    protected EventUseSkill(Int32 id, EventFlags flags) : 
        base(id, flags) {
    }
    public EventUseSkill() : 
        base(25, EventFlags.Server|EventFlags.Client) {
    }
    public new QuantumGame Game {
      get {
        return (QuantumGame)base.Game;
      }
      set {
        base.Game = value;
      }
    }
    public override Int32 GetHashCode() {
      unchecked {
        var hash = 157;
        hash = hash * 31 + Owner.GetHashCode();
        hash = hash * 31 + skill.GetHashCode();
        hash = hash * 31 + weapon.GetHashCode();
        hash = hash * 31 + index.GetHashCode();
        return hash;
      }
    }
  }
  public unsafe partial class EventFix : EventBase {
    public new const Int32 ID = 26;
    protected EventFix(Int32 id, EventFlags flags) : 
        base(id, flags) {
    }
    public EventFix() : 
        base(26, EventFlags.Server|EventFlags.Client) {
    }
    public new QuantumGame Game {
      get {
        return (QuantumGame)base.Game;
      }
      set {
        base.Game = value;
      }
    }
    public override Int32 GetHashCode() {
      unchecked {
        var hash = 163;
        return hash;
      }
    }
  }
  public abstract unsafe partial class EventNexusEvent : EventBase {
    public new const Int32 ID = 27;
    public EntityRef Nexus;
    protected EventNexusEvent(Int32 id, EventFlags flags) : 
        base(id, flags) {
    }
    public new QuantumGame Game {
      get {
        return (QuantumGame)base.Game;
      }
      set {
        base.Game = value;
      }
    }
    public override Int32 GetHashCode() {
      unchecked {
        var hash = 167;
        hash = hash * 31 + Nexus.GetHashCode();
        return hash;
      }
    }
  }
  public unsafe partial class EventOnNexusTakeDamage : EventNexusEvent {
    public new const Int32 ID = 28;
    public FP Damage;
    public EntityRef Source;
    protected EventOnNexusTakeDamage(Int32 id, EventFlags flags) : 
        base(id, flags) {
    }
    public EventOnNexusTakeDamage() : 
        base(28, EventFlags.Server|EventFlags.Client) {
    }
    public override Int32 GetHashCode() {
      unchecked {
        var hash = 173;
        hash = hash * 31 + Nexus.GetHashCode();
        hash = hash * 31 + Damage.GetHashCode();
        hash = hash * 31 + Source.GetHashCode();
        return hash;
      }
    }
  }
  public unsafe partial class EventOnEnableFix : EventBase {
    public new const Int32 ID = 29;
    public EntityRef mechanic;
    public EntityRef nexusIndentifier;
    public QBoolean isEnabled;
    protected EventOnEnableFix(Int32 id, EventFlags flags) : 
        base(id, flags) {
    }
    public EventOnEnableFix() : 
        base(29, EventFlags.Server|EventFlags.Client) {
    }
    public new QuantumGame Game {
      get {
        return (QuantumGame)base.Game;
      }
      set {
        base.Game = value;
      }
    }
    public override Int32 GetHashCode() {
      unchecked {
        var hash = 179;
        hash = hash * 31 + mechanic.GetHashCode();
        hash = hash * 31 + nexusIndentifier.GetHashCode();
        hash = hash * 31 + isEnabled.GetHashCode();
        return hash;
      }
    }
  }
  public unsafe partial class EventOnNexusDestroy : EventNexusEvent {
    public new const Int32 ID = 30;
    public EntityRef Killer;
    protected EventOnNexusDestroy(Int32 id, EventFlags flags) : 
        base(id, flags) {
    }
    public EventOnNexusDestroy() : 
        base(30, EventFlags.Server|EventFlags.Client|EventFlags.Synced) {
    }
    public override Int32 GetHashCode() {
      unchecked {
        var hash = 181;
        hash = hash * 31 + Nexus.GetHashCode();
        hash = hash * 31 + Killer.GetHashCode();
        return hash;
      }
    }
  }
  public unsafe partial class EventOnTeamNexusDestroy : EventBase {
    public new const Int32 ID = 31;
    public Team Team;
    protected EventOnTeamNexusDestroy(Int32 id, EventFlags flags) : 
        base(id, flags) {
    }
    public EventOnTeamNexusDestroy() : 
        base(31, EventFlags.Server|EventFlags.Client|EventFlags.Synced) {
    }
    public new QuantumGame Game {
      get {
        return (QuantumGame)base.Game;
      }
      set {
        base.Game = value;
      }
    }
    public override Int32 GetHashCode() {
      unchecked {
        var hash = 191;
        hash = hash * 31 + Team.GetHashCode();
        return hash;
      }
    }
  }
  public unsafe partial class EventPlayerLeft : EventBase {
    public new const Int32 ID = 32;
    public PlayerRef Player;
    protected EventPlayerLeft(Int32 id, EventFlags flags) : 
        base(id, flags) {
    }
    public EventPlayerLeft() : 
        base(32, EventFlags.Server|EventFlags.Client) {
    }
    public new QuantumGame Game {
      get {
        return (QuantumGame)base.Game;
      }
      set {
        base.Game = value;
      }
    }
    public override Int32 GetHashCode() {
      unchecked {
        var hash = 193;
        hash = hash * 31 + Player.GetHashCode();
        return hash;
      }
    }
  }
  public unsafe partial class EventOnChangeWeapon : EventBase {
    public new const Int32 ID = 33;
    public EntityRef Mechanic;
    public Weapon weapon;
    protected EventOnChangeWeapon(Int32 id, EventFlags flags) : 
        base(id, flags) {
    }
    public EventOnChangeWeapon() : 
        base(33, EventFlags.Server|EventFlags.Client) {
    }
    public new QuantumGame Game {
      get {
        return (QuantumGame)base.Game;
      }
      set {
        base.Game = value;
      }
    }
    public override Int32 GetHashCode() {
      unchecked {
        var hash = 197;
        hash = hash * 31 + Mechanic.GetHashCode();
        hash = hash * 31 + weapon.GetHashCode();
        return hash;
      }
    }
  }
  public unsafe partial class EventOnTrapDestroyed : EventBase {
    public new const Int32 ID = 34;
    public Int32 TrapRefHashCode;
    public EntityRef Mechanic;
    public FPVector3 TrapPosition;
    public AssetRef<TrapData> TrapData;
    protected EventOnTrapDestroyed(Int32 id, EventFlags flags) : 
        base(id, flags) {
    }
    public EventOnTrapDestroyed() : 
        base(34, EventFlags.Server|EventFlags.Client) {
    }
    public new QuantumGame Game {
      get {
        return (QuantumGame)base.Game;
      }
      set {
        base.Game = value;
      }
    }
    public override Int32 GetHashCode() {
      unchecked {
        var hash = 199;
        hash = hash * 31 + TrapRefHashCode.GetHashCode();
        hash = hash * 31 + Mechanic.GetHashCode();
        hash = hash * 31 + TrapPosition.GetHashCode();
        hash = hash * 31 + TrapData.GetHashCode();
        return hash;
      }
    }
  }
  public unsafe partial class EventOnWeaponShoot : EventBase {
    public new const Int32 ID = 35;
    public EntityRef Mechanic;
    protected EventOnWeaponShoot(Int32 id, EventFlags flags) : 
        base(id, flags) {
    }
    public EventOnWeaponShoot() : 
        base(35, EventFlags.Server|EventFlags.Client) {
    }
    public new QuantumGame Game {
      get {
        return (QuantumGame)base.Game;
      }
      set {
        base.Game = value;
      }
    }
    public override Int32 GetHashCode() {
      unchecked {
        var hash = 211;
        hash = hash * 31 + Mechanic.GetHashCode();
        return hash;
      }
    }
  }
  public unsafe partial class EventDummyEvent : EventBase {
    public new const Int32 ID = 36;
    protected EventDummyEvent(Int32 id, EventFlags flags) : 
        base(id, flags) {
    }
    public EventDummyEvent() : 
        base(36, EventFlags.Server|EventFlags.Client) {
    }
    public new QuantumGame Game {
      get {
        return (QuantumGame)base.Game;
      }
      set {
        base.Game = value;
      }
    }
    public override Int32 GetHashCode() {
      unchecked {
        var hash = 223;
        return hash;
      }
    }
  }
}
#pragma warning restore 0109
#pragma warning restore 1591
