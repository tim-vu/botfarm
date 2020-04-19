import { thunk, action, computed, Thunk, Action, Computed } from 'easy-peasy';
import { AccountsApi, AccountVm, UpdateAccount, CreateAccount } from '../api';
import { ById } from './byid';

const API = new AccountsApi();

interface AccountCollection {
    active: AccountVm[];
    banned: AccountVm[];
}

export interface AccountsModel {
    byId: ById<AccountVm>;
    selected: number[];

    accounts: Computed<AccountsModel, AccountCollection>;

    fetchAccounts: Thunk<AccountsModel>;
    addAccount: Thunk<AccountsModel, CreateAccount>;
    updateAccount: Thunk<AccountsModel, UpdateAccount>;
    deleteAccount: Thunk<AccountsModel, number>;

    fetchedAccounts: Action<AccountsModel, AccountVm[]>;
    addedAccount: Action<AccountsModel, AccountVm>;
    updatedAccount: Action<AccountsModel, UpdateAccount>;
    deletedAccount: Action<AccountsModel, number>;

    selectAccounts: Action<AccountsModel, boolean>;
    selectAccount: Action<AccountsModel, number>;
}

const accountsModel: AccountsModel = {
    byId: {},
    selected: [],

    accounts: computed(state => {
        return {
            active: Object.values(state.byId).filter(a => !a.banned),
            banned: Object.values(state.byId).filter(a => a.banned)
        };
    }),

    //Thunks
    fetchAccounts: thunk(async actions => {
        const response = await API.accountsGetAll();
        actions.fetchedAccounts(response.data);
    }),

    addAccount: thunk(async (actions, createaccount) => {
        const response = await API.accountsCreate(createaccount);
        actions.addedAccount(response.data);
    }),

    updateAccount: thunk(async (actions, updateAccount) => {
        await API.accountsUpdate(updateAccount);
        actions.updatedAccount(updateAccount);
    }),

    deleteAccount: thunk(async (actions, id) => {
        await API.accountsDelete(id);
        actions.deletedAccount(id);
    }),

    //Actions
    fetchedAccounts: action((state, accounts) => {
        state.byId = accounts.reduce(
            (acc: ById<AccountVm>, curr: AccountVm) => {
                acc[curr.id] = curr;
                return acc;
            },
            {}
        );
    }),

    addedAccount: action((state, account) => {
        state.byId[account.id] = account;
    }),

    updatedAccount: action((state, updateAccount) => {
        const account = state.byId[updateAccount.id];
        account.mule = updateAccount.mule;
        account.password = updateAccount.password;
    }),

    deletedAccount: action((state, id) => {
        delete state.byId[id];
        state.selected = state.selected.filter(i => i !== id);
    }),

    selectAccounts: action((state, checked) => {
        if (checked) {
            state.selected = state.accounts.active.map(a => a.id);
            return;
        }

        state.selected = [];
    }),

    selectAccount: action((state, id) => {
        const index = state.selected.indexOf(id);

        if (index === -1) {
            state.selected = state.selected.concat(id);
            return;
        }

        state.selected = state.selected.filter(i => i !== id);
    })
};

export default accountsModel;
