import React from 'react';
import GoldCard from './goldcard/GoldCard';
import './Overview.css';
import '../../app/App.css';
import Table from '@material-ui/core/Table';
import TableBody from '@material-ui/core/TableBody';
import TableCell from '@material-ui/core/TableCell';
import TableContainer from '@material-ui/core/TableContainer';
import TableHead from '@material-ui/core/TableHead';
import TableRow from '@material-ui/core/TableRow';
import Paper from '@material-ui/core/Paper';
import Button from '@material-ui/core/Button';
import { Divider } from '@material-ui/core';
import { withStyles } from '@material-ui/core/styles';
import { useStoreState } from 'easy-peasy';

const MULES = [
    {
        username: 'email@email.com',
        password: 'mypassword',
        proxy: '192.168.1.1:8000',
        startTime: new Date(),
        gold: 500 * 1000,
        inGame: true,
        world: 103
    }
];

const ColorButton = withStyles(() => ({
    root: {
        backgroundColor: '#1976d2',
        '&:hover': {
            backgroundColor: '#0069d9',
            borderColor: '#0062cc',
            boxShadow: 'none'
        }
    }
}))(Button);

const Overview = () => {
    const status = useStoreState(state => state.status.status);

    return (
        <div className="overview">
            <div className="cards page-section">
                <GoldCard title="Session earnings" amount={500 * 1000} />
                <GoldCard title="Current gold" amount={25 * 1000 * 1000} />
                <GoldCard title="Hourly earnings" amount={5 * 1000 * 1000} />
            </div>
            <div className="controls page-section">
                <h3>Controls</h3>
                <Paper className="controls-paper">
                    <ColorButton
                        size="medium"
                        color="inherit"
                        disabled={!status.canStart}
                    >
                        Start
                    </ColorButton>
                    <ColorButton
                        size="medium"
                        color="inherit"
                        disabled={!status.running}
                    >
                        Stop
                    </ColorButton>
                    <ColorButton
                        size="medium"
                        color="inherit"
                        disabled={!status.running}
                    >
                        Initiate muling
                    </ColorButton>
                </Paper>
            </div>
            <div className="page-section">
                <Divider />
            </div>
            <div className="page-section">
                <h3>Mules</h3>
                <TableContainer
                    component={Paper}
                    color="inherit"
                    className="mule-table"
                >
                    <Table>
                        <TableHead>
                            <TableRow>
                                <TableCell>Username</TableCell>
                                <TableCell>Password</TableCell>
                                <TableCell>Proxy</TableCell>
                                <TableCell>Start Time</TableCell>
                                <TableCell>Gold</TableCell>
                                <TableCell>In game</TableCell>
                                <TableCell>World</TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {MULES.map(m => (
                                <TableRow>
                                    <TableCell>{m.username}</TableCell>
                                    <TableCell>{m.password}</TableCell>
                                    <TableCell>{m.proxy}</TableCell>
                                    <TableCell>
                                        {m.startTime.toLocaleTimeString()}
                                    </TableCell>
                                    <TableCell>{m.gold}</TableCell>
                                    <TableCell>{m.inGame.toString()}</TableCell>
                                    <TableCell>{m.world}</TableCell>
                                </TableRow>
                            ))}
                        </TableBody>
                    </Table>
                </TableContainer>
            </div>
            <div className="page-section">
                <Divider />
            </div>
            <div className="fixed-table page-section">
                <h3>Bots</h3>
                <TableContainer
                    component={Paper}
                    color="inherit"
                    className="fixed-table"
                >
                    <Table>
                        <TableHead>
                            <TableRow>
                                <TableCell>Username</TableCell>
                                <TableCell>Password</TableCell>
                                <TableCell>Proxy</TableCell>
                                <TableCell>Start Time</TableCell>
                                <TableCell>Gold</TableCell>
                                <TableCell>In game</TableCell>
                                <TableCell>World</TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {MULES.map(m => (
                                <TableRow>
                                    <TableCell>{m.username}</TableCell>
                                    <TableCell>{m.password}</TableCell>
                                    <TableCell>{m.proxy}</TableCell>
                                    <TableCell>
                                        {m.startTime.toLocaleTimeString()}
                                    </TableCell>
                                    <TableCell>{m.gold}</TableCell>
                                    <TableCell>{m.inGame.toString()}</TableCell>
                                    <TableCell>{m.world}</TableCell>
                                </TableRow>
                            ))}
                        </TableBody>
                    </Table>
                </TableContainer>
            </div>
        </div>
    );
};

export default Overview;
