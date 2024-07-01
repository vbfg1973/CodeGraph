using CodeGraph.Clients;
using CodeGraph.Clients.Dto.FileSystem;
using CodeGraph.UiServer.Features.FileSystem.Helpers;
using CodeGraph.UiServer.Features.FileSystem.Models;
using Fluxor;

namespace CodeGraph.UiServer.Features.FileSystem
{
    public record FileSystemState
    {
        public bool Initialized { get; init; }
        public bool Loading { get; init; }
        public FileSystemHierarchyDto[] FileSystemHierarchies { get; init; }
        public FileSystemTreeItemData[] FileSystemTreeItemList { get; init; }
    }

    public class FileSystemFeature : Feature<FileSystemState>
    {
        public override string GetName()
        {
            return "FileSystem";
        }

        protected override FileSystemState GetInitialState()
        {
            return new FileSystemState
            {
                Initialized = false,
                Loading = false,
                FileSystemHierarchies = []
            };
        }
    }

    public static class FileSystemReducers
    {
        [ReducerMethod]
        public static FileSystemState OnSetForecasts(FileSystemState state, FileSystemSetHierarchyAction action)
        {
            return state with
            {
                FileSystemHierarchies = action.Hierarchies,
                Loading = false
            };
        }

        [ReducerMethod]
        public static FileSystemState OnSetTreeItems(FileSystemState state, FileSystemSetMappedTreeItems action)
        {
            return state with
            {
                FileSystemTreeItemList = action.TreeItems,
                Loading = false
            };
        }

        [ReducerMethod(typeof(FileSystemSetInitializedAction))]
        public static FileSystemState OnSetInitialized(FileSystemState state)
        {
            return state with
            {
                Initialized = true
            };
        }

        [ReducerMethod(typeof(FileSystemLoadHierarchyAction))]
        public static FileSystemState OnLoadFileSystem(FileSystemState state)
        {
            return state with
            {
                Loading = true
            };
        }
    }

    public class FileSystemEffects(
        IState<FileSystemState> state,
        CodeGraphFileSystemClient fileSystemClient,
        ILogger<FileSystemEffects> logger)
    {
        [EffectMethod(typeof(FileSystemLoadHierarchyAction))]
        public async Task LoadFileSystemHierarchies(IDispatcher dispatcher)
        {
            List<FileSystemHierarchyDto> hierarchies = await fileSystemClient.GetHierarchy();

            dispatcher.Dispatch(new FileSystemSetHierarchyAction(hierarchies.ToArray()));
            dispatcher.Dispatch(new FileSystemMapHierarchiesToTreeItems());
        }

        [EffectMethod(typeof(FileSystemMapHierarchiesToTreeItems))]
        public async Task MapHierarchies(IDispatcher dispatcher)
        {
            List<FileSystemTreeItemData> treeItems = await HierarchyMapper.Map(state.Value.FileSystemHierarchies);
            dispatcher.Dispatch(new FileSystemSetMappedTreeItems(treeItems.ToArray()));
        }
    }

    #region FileSystemActions

    public record FileSystemSetInitializedAction;

    public record FileSystemLoadHierarchyAction;

    public record FileSystemSetHierarchyAction(FileSystemHierarchyDto[] Hierarchies);

    public record FileSystemMapHierarchiesToTreeItems;

    public record FileSystemSetMappedTreeItems(FileSystemTreeItemData[] TreeItems);

    #endregion
}