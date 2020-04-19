import '../../../app/App';
import Modal from '../../../../components/modal/Modal';

import React from 'react';

import { useForm } from 'react-hook-form';

import Paper from '@material-ui/core/Paper';
import TextField from '@material-ui/core/TextField';

export interface ImportAccountsProps {
    show: boolean;
    handleClose: Function;
}

interface FormData {
    accountList: string;
}

export default ({ show, handleClose }: ImportAccountsProps) => {
    const { register, handleSubmit, errors, reset } = useForm<FormData>();

    const onSubmit = (data: FormData) => {};

    return (
        <Modal show={show} modalClassName="account-modal">
            <Paper className="modal-form">
                <form onSubmit={handleSubmit(onSubmit)}>
                    <div className="modal-form-item">
                        <TextField
                            name="accountList"
                            label="Accounts"
                            type="text"
                            fullWidth={true}
                            multiline
                            inputRef={register({
                                required: 'Accounts are required'
                            })}
                            error={errors.accountList !== undefined}
                            helperText={
                                errors.accountList && errors.accountList.message
                            }
                        />
                    </div>
                </form>
            </Paper>
        </Modal>
    );
};
