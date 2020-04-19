import React from 'react';

import '../../../app/App.css';
import './CreateProxy.css';
import { valueInRange, transformInt } from '../../../../common/FormUtils';
import { useStoreActions } from '../../../../store';
import { CreateProxy } from '../../../../api';
import Modal from '../../../modal/Modal';

import Paper from '@material-ui/core/Paper';
import TextField from '@material-ui/core/TextField';
import Button from '@material-ui/core/Button';

import { useForm, Controller } from 'react-hook-form';

interface CreateProxyProps {
    show: boolean;
    handleClose: Function;
}

const MAX_PORT = 65535;
const MIN_PORT = 1;

const IP_ADDRESS = /^(?!0)(?!.*\.$)((1?\d?\d|25[0-5]|2[0-4]\d)(\.|$)){4}$/;

const CreateProxyModal: React.FC<CreateProxyProps> = ({
    show,
    handleClose
}) => {
    const {
        register,
        errors,
        handleSubmit,
        control,
        reset,
        getValues,
        triggerValidation
    } = useForm<CreateProxy>({
        mode: 'onBlur'
    });

    const addProxy = useStoreActions(state => state.proxy.addProxy);

    const closeModal = () => {
        reset();
        handleClose();
    };

    const onSubmit = (data: CreateProxy) => {
        console.log(data);
        addProxy(data);
    };

    return (
        <div>
            <Modal show={show} modalClassName="add-proxy-modal">
                <Paper className="modal-form">
                    <form onSubmit={handleSubmit(onSubmit)}>
                        <h2>Add a proxy</h2>
                        <div className="modal-form-item">
                            <TextField
                                name="ip"
                                label="Ip"
                                fullWidth={true}
                                variant="filled"
                                inputRef={register({
                                    required: 'You must enter an ip address',
                                    pattern: {
                                        value: IP_ADDRESS,
                                        message:
                                            'The given ip address is not valid'
                                    }
                                })}
                                error={errors.ip !== undefined}
                                helperText={errors.ip && errors.ip.message}
                            />
                        </div>
                        <div className="modal-form-item">
                            <Controller
                                as={TextField}
                                control={control}
                                name="port"
                                label="Port"
                                fullWidth={true}
                                variant="filled"
                                rules={{
                                    required: 'You must enter a port number',
                                    validate: valueInRange(MIN_PORT, MAX_PORT)
                                }}
                                onChange={transformInt}
                                error={errors.port !== undefined}
                                helperText={errors.port && errors.port.message}
                            />
                        </div>
                        <div className="modal-form-item">
                            <TextField
                                name="username"
                                label="Username"
                                fullWidth={true}
                                variant="filled"
                                inputRef={register({
                                    validate: value =>
                                        value !== '' ||
                                        getValues().password === '' ||
                                        'You must enter a username as well'
                                })}
                                onChange={() => triggerValidation('password')}
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
                                fullWidth={true}
                                variant="filled"
                                inputRef={register({
                                    validate: value =>
                                        value !== '' ||
                                        getValues().username === '' ||
                                        'You must enter a password as well'
                                })}
                                onChange={() => triggerValidation('username')}
                                error={errors.password !== undefined}
                                helperText={
                                    errors.password && errors.password.message
                                }
                            />
                        </div>
                        <div className="modal-form-controls">
                            <Button
                                variant="contained"
                                color="secondary"
                                size="large"
                                onClick={closeModal}
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
        </div>
    );
};

export default CreateProxyModal;
