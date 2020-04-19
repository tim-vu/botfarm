import { createTypedHooks } from 'easy-peasy';
import { ForFarmStore } from './store';

const {
    useStoreActions,
    useStoreState,
    useStoreDispatch,
    useStore
} = createTypedHooks<ForFarmStore>();

export { useStoreActions, useStoreState, useStoreDispatch, useStore };
