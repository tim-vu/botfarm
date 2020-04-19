import './Settings.css';
import { valueInRange, transformInt } from '../../../common/FormUtils';
import { useStoreState, useStoreActions } from '../../../store';
import { SettingsVm } from '../../../api';

import React from 'react';
import {
    FormControlLabel,
    Checkbox,
    TextField,
    Paper,
    Button
} from '@material-ui/core';
import SaveIcon from '@material-ui/icons/Save';
import SettingsRestore from '@material-ui/icons/SettingsBackupRestore';
import InputAdornment from '@material-ui/core/InputAdornment';
import { useForm, Controller } from 'react-hook-form';

export default () => {
    const settings: SettingsVm = useStoreState(state => state.setting.settings);
    const updateSettings = useStoreActions(
        state => state.setting.updateSettings
    );

    const {
        control,
        register,
        handleSubmit,
        reset,
        getValues,
        errors,
        triggerValidation
    } = useForm<SettingsVm>({
        mode: 'onBlur',
        defaultValues: settings ? settings : undefined,
        validateCriteriaMode: 'all'
    });

    const onSubmit = (data: SettingsVm) => {
        console.log(data);
        updateSettings(data);
    };

    return (
        <div className="settings">
            <form onSubmit={handleSubmit(onSubmit)}>
                <div className="settings-section">
                    <h3>Proxies</h3>
                    <Paper className="settings-paper">
                        <div className="section-item">
                            <FormControlLabel
                                control={
                                    <Controller
                                        as={Checkbox}
                                        name="useProxies"
                                        control={control}
                                        defaultValue={settings.useProxies}
                                    />
                                }
                                label="Use Proxies"
                            />
                        </div>
                        <div className="section-horizontal">
                            <div className="section-item form-text-field">
                                <Controller
                                    as={TextField}
                                    control={control}
                                    name="concurrentAccountsPerProxy"
                                    label="Concurrent accounts per proxy"
                                    type="number"
                                    variant="filled"
                                    fullWidth={true}
                                    rules={{
                                        required: 'This field is required',
                                        validate: valueInRange(1, 25)
                                    }}
                                    onChange={transformInt}
                                    error={
                                        errors.concurrentAccountsPerProxy !==
                                        undefined
                                    }
                                    helperText={
                                        errors.concurrentAccountsPerProxy &&
                                        errors.concurrentAccountsPerProxy
                                            .message
                                    }
                                />
                            </div>
                            <div className="section-item form-text-field">
                                <Controller
                                    as={TextField}
                                    control={control}
                                    name="maxAccountsPerProxy"
                                    label="Max accounts per proxy"
                                    variant="filled"
                                    fullWidth={true}
                                    rules={{
                                        required: 'This field is required',
                                        validate: {
                                            inRange: valueInRange(1, 50),
                                            greaterThan: value =>
                                                value >=
                                                    getValues()
                                                        .concurrentAccountsPerProxy ||
                                                'This value must be greater than concurrent accounts per proxy'
                                        }
                                    }}
                                    onChange={transformInt}
                                    error={
                                        errors.maxAccountsPerProxy !== undefined
                                    }
                                    helperText={
                                        errors.maxAccountsPerProxy &&
                                        errors.maxAccountsPerProxy.message
                                    }
                                />
                            </div>
                        </div>
                    </Paper>
                </div>
                <div className="settings-section">
                    <h3>Instances</h3>
                    <Paper className="settings-paper">
                        <div className="section-horizontal">
                            <div className="form-text-field section-item">
                                <Controller
                                    as={TextField}
                                    control={control}
                                    name="maxActiveBots"
                                    label="Max active bots"
                                    variant="filled"
                                    fullWidth={true}
                                    rules={{
                                        required: 'This field is required',
                                        validate: valueInRange(1, 1000)
                                    }}
                                    onChange={transformInt}
                                />
                            </div>
                            <div className="form-text-field section-item">
                                <Controller
                                    as={TextField}
                                    control={control}
                                    name="minActiveMules"
                                    label="Min active mules"
                                    variant="filled"
                                    fullWidth={true}
                                    rules={{
                                        required: 'This field is required',
                                        validate: valueInRange(1, 50)
                                    }}
                                    onChange={transformInt}
                                />
                            </div>
                            <div className="form-text-field section-item">
                                <Controller
                                    as={TextField}
                                    control={control}
                                    name="maxActiveMules"
                                    label="Max active mules"
                                    variant="filled"
                                    fullWidth={true}
                                    rules={{
                                        validate: value =>
                                            value >=
                                                getValues().minActiveMules ||
                                            'This value must be greater than or equal to min active mules'
                                    }}
                                    onChange={transformInt}
                                    error={errors.maxActiveMules !== undefined}
                                    helperText={
                                        errors.maxActiveMules &&
                                        errors.maxActiveMules.message
                                    }
                                />
                            </div>
                        </div>
                    </Paper>
                </div>
                <div className="settings-section">
                    <h3>RSPeer</h3>
                    <Paper className="settings-paper">
                        <div className="section-horizontal">
                            <div className="form-text-field section-item">
                                <TextField
                                    name="hostname"
                                    label="Hostname"
                                    type="text"
                                    variant="filled"
                                    fullWidth={true}
                                    inputRef={register()}
                                />
                            </div>
                            <div className="section-item form-text-field">
                                <Controller
                                    as={TextField}
                                    control={control}
                                    name="launchSleep"
                                    label="Launch sleep"
                                    variant="filled"
                                    fullWidth={true}
                                    InputProps={{
                                        startAdornment: (
                                            <InputAdornment position="start">
                                                ms
                                            </InputAdornment>
                                        )
                                    }}
                                    rules={{
                                        required: 'This field is required',
                                        validate: valueInRange(1000, 20000)
                                    }}
                                    onChange={transformInt}
                                />
                            </div>
                        </div>
                        <div className="form-text-field section-item api-key">
                            <TextField
                                name="apiKey"
                                label="Api key"
                                type="text"
                                variant="filled"
                                fullWidth={true}
                                inputRef={register()}
                            />
                        </div>
                    </Paper>
                </div>
                <div className="settings-section">
                    <h3>Script</h3>
                    <Paper className="settings-paper">
                        <div className="section-horizontal">
                            <div className="form-text-field section-item">
                                <TextField
                                    name="botScriptName"
                                    label="Bot script name"
                                    variant="filled"
                                    fullWidth={true}
                                    inputRef={register()}
                                />
                            </div>
                            <div className="form-text-field section-item">
                                <TextField
                                    name="muleScriptName"
                                    label="Mule script name"
                                    variant="filled"
                                    fullWidth={true}
                                    inputRef={register()}
                                />
                            </div>
                        </div>
                    </Paper>
                </div>
                <div className="settings-section">
                    <h3>Muling</h3>
                    <Paper className="settings-paper">
                        <div className="form-text-field section-item">
                            <Controller
                                as={TextField}
                                control={control}
                                name="muleIntervalMinutes"
                                label="Mule interval"
                                variant="filled"
                                fullWidth={true}
                                InputProps={{
                                    startAdornment: (
                                        <InputAdornment position="start">
                                            m
                                        </InputAdornment>
                                    )
                                }}
                                rules={{
                                    required: 'This field is required',
                                    validate: valueInRange(30, 300)
                                }}
                                onChange={transformInt}
                            />
                        </div>
                    </Paper>
                </div>
                <div className="settings-controls">
                    <Button
                        variant="contained"
                        color="secondary"
                        size="large"
                        startIcon={<SettingsRestore />}
                        onClick={() => reset()}
                    >
                        Reset
                    </Button>
                    <Button
                        variant="contained"
                        color="primary"
                        size="large"
                        startIcon={<SaveIcon />}
                        type="submit"
                    >
                        Save
                    </Button>
                </div>
            </form>
        </div>
    );
};
