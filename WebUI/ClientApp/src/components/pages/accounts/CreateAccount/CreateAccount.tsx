import './CreateAccount.css';
import '../../../app/App';

import React from 'react';
import { useForm, Controller } from 'react-hook-form';
import FormControlLabel from '@material-ui/core/FormControlLabel';
import Switch from '@material-ui/core/Switch';
import TextField from '@material-ui/core/TextField';
import Paper from '@material-ui/core/Paper';
import Button from '@material-ui/core/Button';
import { useStoreState, useStoreActions } from '../../../../store';

import Modal from '../../../modal/Modal';
import { CreateAccount } from '../../../../api';
import { transformInt } from "../../../../common/FormUtils";

export interface CreateAccountProps {
    show: boolean;
    handleClose: Function;
}

export default ({ show, handleClose }: CreateAccountProps) => {
    const byId = useStoreState(state => state.account.byId);
    const addAccount = useStoreActions(state => state.account.addAccount);

    const isUsernameFree = (username: string): boolean =>
        Object.values(byId).filter(a => a.username === username).length === 0;

    const { register, handleSubmit, errors, reset, control } = useForm<
        CreateAccount
    >({
        mode: 'onBlur'
    });

    const onSubmit = (data: CreateAccount) => {
        addAccount(data);
        reset();
        handleClose();
    };

    return (
        <Modal show={show} modalClassName="account-modal">
            <Paper className="modal-form">
                <form onSubmit={handleSubmit(onSubmit)}>
                    <h2>Add an account</h2>
                    <div className="modal-form-item">
                        <TextField
                            name="username"
                            label="Username"
                            type="text"
                            variant="filled"
                            fullWidth={true}
                            inputRef={register({
                                required: 'A username is required',
                                validate: value =>
                                    isUsernameFree(value) ||
                                    'An account with this username already exists',
                                minLength: {
                                    value: 3,
                                    message:
                                        'A username must be at least 3 characters long'
                                }
                            })}
                            error={errors.username !== undefined}
                            helperText={
                                errors.username && errors.username.message
                            }
                        />
                    </div>
                    <div className="modal-form-item">
                        <TextField
                            name="password"
                            label="Password"
                            type="password"
                            variant="filled"
                            fullWidth={true}
                            inputRef={register({
                                required: 'A password is required',
                                minLength: {
                                    value: 3,
                                    message:
                                        'A password must be at least 3 characters long'
                                }
                            })}
                            error={errors.password !== undefined}
                            helperText={
                                errors.password && errors.password.message
                            }
                        />
                    </div>
                    <div className="modal-form-item">
                        <FormControlLabel
                            control={
                                <Controller
                                    as={Switch}
                                    name="mule"
                                    control={control}
                                    defaultValue={false}
                                />
                            }
                            label="Mule"
                        />
                    </div>
                    <div className="modal-form-item">
                        <Controller
                            as={TextField}
                            control={control}
                            name="remainingMembershipDays"
                            label="Membership days remaining"
                            type="number"
                            fullWidth={true}
                            variant="filled"
                            inputProps={{ min: 0, max: 60 }}
                            defaultValue={0}
                            rules={{ required: true }}
                            onChange={transformInt}
                            error={errors.remainingMembershipDays !== undefined}
                            helperText={
                                errors.remainingMembershipDays &&
                                errors.remainingMembershipDays.message
                            }
                        />
                    </div>
                    <div className="modal-form-items account-form-controls">
                        <Button
                            variant="contained"
                            color="secondary"
                            size="large"
                            onClick={() => {
                                reset();
                                handleClose();
                            }}
                        >
                            Cancel
                        </Button>
                        <Button
                            variant="contained"
                            color="primary"
                            size="large"
                            type="submit"
                        >
                            Add
                        </Button>
                    </div>
                </form>
            </Paper>
        </Modal>
    );
};
