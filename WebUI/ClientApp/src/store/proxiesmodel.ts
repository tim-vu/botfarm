import { thunk, action, computed, Computed, Thunk, Action } from 'easy-peasy';
import { CreateProxy, ProxiesApi, ProxyVm } from '../api';
import { ById } from './byid';

const API = new ProxiesApi();

interface ProxyCollection {
    active: ProxyVm[];
    used: ProxyVm[];
}

export interface ProxiesModel {
    byId: ById<ProxyVm>;
    selected: number[];

    proxies: Computed<ProxiesModel, ProxyCollection>;

    fetchProxies: Thunk<ProxiesModel>;
    addProxy: Thunk<ProxiesModel, CreateProxy>;
    deleteProxy: Thunk<ProxiesModel, number>;

    fetchedProxies: Action<ProxiesModel, ProxyVm[]>;
    addedProxy: Action<ProxiesModel, ProxyVm>;
    deletedProxy: Action<ProxiesModel, number>;

    selectProxies: Action<ProxiesModel, boolean>;
    selectProxy: Action<ProxiesModel, number>;
}

const proxiesModel: ProxiesModel = {
    byId: {},
    selected: [],

    proxies: computed(state => {
        return {
            active: Object.values(state.byId).filter(p => !p.used),
            used: Object.values(state.byId).filter(p => p.used)
        };
    }),

    //Thunks
    fetchProxies: thunk(async actions => {
        const response = await API.proxiesGetAll();
        actions.fetchedProxies(response.data);
    }),

    addProxy: thunk(async (actions, createProxy) => {
        const response = await API.proxiesCreate(createProxy);
        actions.addedProxy(response.data);
    }),

    deleteProxy: thunk(async (actions, id) => {
        await API.proxiesDelete(id);
        actions.deletedProxy(id);
    }),

    //Actions
    fetchedProxies: action((state, proxies) => {
        state.byId = proxies;
    }),

    addedProxy: action((state, proxy) => {
        state.byId[proxy.id] = proxy;
    }),

    deletedProxy: action((state, id) => {
        delete state.byId[id];
    }),

    selectProxies: action((state, checked) => {
        if (checked) {
            state.selected = state.proxies.active.map(p => p.id);
            return;
        }

        state.selected = [];
    }),

    selectProxy: action((state, id) => {
        const index = state.selected.indexOf(id);

        if (index === -1) {
            state.selected = state.selected.concat(id);
            return;
        }

        state.selected = state.selected.filter(i => i !== id);
    })
};

export default proxiesModel;
