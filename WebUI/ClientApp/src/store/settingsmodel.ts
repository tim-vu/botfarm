import { SettingsApi, SettingsVm } from '../api';
import { action, Action, thunk, Thunk } from 'easy-peasy';

const API = new SettingsApi();

export interface SettingsModel {
    settings: SettingsVm;

    fetchSettings: Thunk<SettingsModel>;
    updateSettings: Thunk<SettingsModel, SettingsVm>;
    setSettings: Action<SettingsModel, SettingsVm>;
}

const defaultValues: SettingsVm = {
    concurrentAccountsPerProxy: 10,
    maxAccountsPerProxy: 30,
    launchSleep: 5000,
    maxActiveBots: 25,
    maxActiveMules: 3,
    minActiveMules: 1,
    muleIntervalMinutes: 90,
    useProxies: true
};

const settingsModel: SettingsModel = {
    settings: defaultValues,

    fetchSettings: thunk(async actions => {
        const response = await API.settingsGet();
        actions.setSettings(response.data);
    }),

    updateSettings: thunk(async (actions, settings) => {
        await API.settingsUpdate(settings);
        actions.setSettings(settings);
    }),

    setSettings: action((state, settings) => {
        state.settings = settings;
    })
};

export default settingsModel;
