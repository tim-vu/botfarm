import { Thunk, Action, thunk, action } from 'easy-peasy';
import { StatusApi, StatusVm, UpdateStatus } from '../api';

const STATUS_API = new StatusApi();

export interface StatusModel {

  status: StatusVm;

  fetchStatus: Thunk<StatusModel>;
  updateStatus: Thunk<StatusModel, UpdateStatus>;

  setStatus: Action<StatusModel, StatusVm>;
}

const statusModel : StatusModel = {

  status: { running: false, canStart: false},

  fetchStatus: thunk(async actions => {
    const response = await STATUS_API.statusGet();
    actions.setStatus(response.data);
  }),

  updateStatus: thunk(async (actions, status) => {
    const response = await STATUS_API.statusUpdate(status);
    actions.setStatus(response.data);
  }),

  setStatus: action((state, status) => {
    state.status = status;
  })

};

export default statusModel;