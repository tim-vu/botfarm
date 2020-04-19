import { createStore } from 'easy-peasy';
import accountsModel, { AccountsModel } from './accountmodel';
import proxiesModel, { ProxiesModel } from './proxiesmodel';
import settingsModel, { SettingsModel } from './settingsmodel';
import statusModel, { StatusModel } from './statusmodel';

export interface ForFarmStore {
    account: AccountsModel;
    proxy: ProxiesModel;
    setting: SettingsModel;
    status: StatusModel;
}

export default createStore<ForFarmStore>({
    account: accountsModel,
    proxy: proxiesModel,
    setting: settingsModel,
    status: statusModel
});
