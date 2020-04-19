import React from 'react';

import '../../app/App.css';
import './Proxies.css';

import { useStoreState, useStoreActions } from '../../../store';
import { ProxyVm } from '../../../api';
import CreateProxyModal from './createproxy/CreateProxy';

import EnhancedTableHead, {
    Order,
    HeadCell
} from '../../enhancedtablehead/EnhancedTableHead';

import Table from '@material-ui/core/Table';
import TableBody from '@material-ui/core/TableBody';
import TableCell from '@material-ui/core/TableCell';
import TableContainer from '@material-ui/core/TableContainer';
import TableHead from '@material-ui/core/TableHead';
import TableRow from '@material-ui/core/TableRow';
import Paper from '@material-ui/core/Paper';
import Toolbar from '@material-ui/core/Toolbar';
import Typography from '@material-ui/core/Typography';
import Checkbox from '@material-ui/core/Checkbox';
import AddBoxIcon from '@material-ui/icons/AddBox';
import DeleteIcon from '@material-ui/icons/Delete';
import Tooltip from '@material-ui/core/Tooltip';
import IconButton from '@material-ui/core/IconButton';

import { Icon } from '@material-ui/core';

function descendingComparator<T>(a: T, b: T, orderBy: keyof T) {
    if (b[orderBy] < a[orderBy]) {
        return -1;
    }
    if (b[orderBy] > a[orderBy]) {
        return 1;
    }
    return 0;
}

function getComparator<T>(
    order: Order,
    orderBy: keyof T
): (a: T, b: T) => number {
    return order === 'desc'
        ? (a, b) => descendingComparator(a, b, orderBy)
        : (a, b) => -descendingComparator(a, b, orderBy);
}

function stableSort<T>(array: T[], comparator: (a: T, b: T) => number) {
    const stabilizedThis = array.map((el, index) => [el, index] as [T, number]);
    stabilizedThis.sort((a, b) => {
        const order = comparator(a[0], b[0]);
        if (order !== 0) return order;
        return a[1] - b[1];
    });
    return stabilizedThis.map(el => el[0]);
}

const HEAD_CELLS: HeadCell<ProxyVm>[] = [
    {
        name: 'ip',
        label: 'Ip',
        disablePadding: true,
        numeric: false
    },
    {
        name: 'port',
        label: 'Port',
        disablePadding: false,
        numeric: true
    },
    {
        name: 'currentAccounts',
        label: 'Current accounts',
        disablePadding: false,
        numeric: true
    },
    {
        name: 'previousAccounts',
        label: 'Previous accounts',
        disablePadding: false,
        numeric: true
    }
];

const Proxies = () => {
    const proxies = useStoreState(state => state.proxy.proxies);
    const selected = useStoreState(state => state.proxy.selected);

    const selectProxies = useStoreActions(state => state.proxy.selectProxies);
    const selectProxy = useStoreActions(state => state.proxy.selectProxy);

    const deleteProxy = useStoreActions(state => state.proxy.deleteProxy);

    const [show, setShow] = React.useState(false);
    const [order, setOrder] = React.useState<Order>('asc');
    const [orderBy, setOrderBy] = React.useState<keyof ProxyVm>('ip');

    const selectProxiesClicked = (
        event: React.ChangeEvent<HTMLInputElement>
    ) => {
        selectProxies(event.target.checked);
    };

    const requestSortClicked = (
        _event: React.MouseEvent<unknown>,
        name: keyof ProxyVm
    ) => {
        const isAsc = orderBy === name && order === 'asc';
        setOrder(isAsc ? 'desc' : 'asc');
        setOrderBy(name);
    };

    const deleteProxyClicked = () => {
        selected.forEach(i => deleteProxy(i));
    };

    const addProxyClicked = () => {
        setShow(true);
    };

    const cancelAddProxyClicked = () => {
        setShow(false);
    };

    const isSelected = (id: number) => selected.indexOf(id) !== -1;

    return (
        <div className="proxies">
            <div className="page-section">
                <CreateProxyModal
                    show={show}
                    handleClose={cancelAddProxyClicked}
                />
                <div className="table-toolbar">
                    <h3 className="table-toolbar-title">Active proxies</h3>
                    {selected.length > 0 ? (
                        <Tooltip title="Delete">
                            <IconButton onClick={deleteProxyClicked}>
                                <DeleteIcon />
                            </IconButton>
                        </Tooltip>
                    ) : (
                        <Tooltip title="Add">
                            <IconButton onClick={addProxyClicked}>
                                <AddBoxIcon />
                            </IconButton>
                        </Tooltip>
                    )}
                </div>
                <TableContainer
                    component={Paper}
                    color="inherit"
                    className="fixed-table"
                >
                    <Table stickyHeader>
                        <EnhancedTableHead
                            headCells={HEAD_CELLS}
                            numSelected={selected.length}
                            order={order}
                            orderBy={orderBy}
                            rowCount={proxies.active.length}
                            onSelectAllClick={selectProxiesClicked}
                            onRequestSort={requestSortClicked}
                        />
                        <TableBody>
                            {stableSort(
                                proxies.active,
                                getComparator(order, orderBy)
                            ).map(p => {
                                const isRowSelected = isSelected(p.id);

                                return (
                                    <TableRow
                                        key={p.username}
                                        hover
                                        onClick={() => selectProxy(p.id)}
                                        role="checkbox"
                                        selected={isRowSelected}
                                    >
                                        <TableCell padding="checkbox">
                                            <Checkbox checked={isRowSelected} />
                                        </TableCell>
                                        <TableCell>{p.ip}</TableCell>
                                        <TableCell>{p.port}</TableCell>
                                        <TableCell>
                                            {p.currentAccounts}
                                        </TableCell>
                                        <TableCell>
                                            {p.previousAccounts}
                                        </TableCell>
                                    </TableRow>
                                );
                            })}
                        </TableBody>
                    </Table>
                </TableContainer>
            </div>
            <div className="page-section">
                <TableContainer
                    component={Paper}
                    color="inherit"
                    className="fixed-table"
                >
                    <Toolbar>
                        <Typography variant="h6">Used Proxies</Typography>
                    </Toolbar>
                    <Table stickyHeader>
                        <TableHead>
                            <TableRow>
                                <TableCell>Ip</TableCell>
                                <TableCell>Port</TableCell>
                                <TableCell>Current Accounts</TableCell>
                                <TableCell>Previous Accounts</TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {proxies.used.map(p => (
                                <TableRow>
                                    <TableCell>{p.ip}</TableCell>
                                    <TableCell>{p.port}</TableCell>
                                    <TableCell>{p.currentAccounts}</TableCell>
                                    <TableCell>{p.previousAccounts}</TableCell>
                                </TableRow>
                            ))}
                        </TableBody>
                    </Table>
                </TableContainer>
            </div>
        </div>
    );
};

export default Proxies;
