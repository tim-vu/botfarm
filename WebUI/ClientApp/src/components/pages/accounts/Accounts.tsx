import '../../app/App.css';
import './Accounts.css';
import CreateAccount from './createaccount/CreateAccount';
import EnhancedTableHead, {
    Order,
    HeadCell
} from '../../enhancedtablehead/EnhancedTableHead';
import React from 'react';
import Checkbox from '@material-ui/core/Checkbox';
import AddBoxIcon from '@material-ui/icons/AddBox';
import DeleteIcon from '@material-ui/icons/Delete';
import Tooltip from '@material-ui/core/Tooltip';
import IconButton from '@material-ui/core/IconButton';
import Table from '@material-ui/core/Table';
import TableBody from '@material-ui/core/TableBody';
import TableCell from '@material-ui/core/TableCell';
import TableContainer from '@material-ui/core/TableContainer';
import TableHead from '@material-ui/core/TableHead';
import TableRow from '@material-ui/core/TableRow';
import Paper from '@material-ui/core/Paper';
import Toolbar from '@material-ui/core/Toolbar';
import Typography from '@material-ui/core/Typography';
import { useStoreState, useStoreActions } from '../../../store';
import { AccountVm, Skill } from '../../../api';

const MINUTES_IN_HOUR = 60;
const MINUTES_IN_DAY = 24 * MINUTES_IN_HOUR;

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

const headCells: HeadCell<AccountVm>[] = [
    {
        name: 'username',
        numeric: false,
        disablePadding: true,
        label: 'Username'
    },
    {
        name: 'password',
        numeric: false,
        disablePadding: false,
        label: 'Password'
    },
    {
        name: 'totalLevel',
        numeric: true,
        disablePadding: false,
        label: 'Total Level'
    },
    {
        name: 'goldEarned',
        numeric: true,
        disablePadding: false,
        label: 'Gold Earned'
    },
    {
        name: 'runtimeMinutes',
        numeric: true,
        disablePadding: false,
        label: 'Runtime'
    }
];

const Accounts: React.FC<{}> = () => {
    const [show, setShow] = React.useState(false);

    const [order, setOrder] = React.useState<Order>('asc');
    const [orderBy, setOrderBy] = React.useState<keyof AccountVm>('username');

    const selectAccounts = useStoreActions(
        state => state.account.selectAccounts
    );
    const selectAccount = useStoreActions(state => state.account.selectAccount);
    const deleteAccount = useStoreActions(state => state.account.deleteAccount);

    const accounts = useStoreState(state => state.account.accounts);
    const selected = useStoreState(state => state.account.selected);

    const handleRequestSort = (
        event: React.MouseEvent<unknown>,
        name: keyof AccountVm
    ) => {
        const isAsc = orderBy === name && order === 'asc';
        setOrder(isAsc ? 'desc' : 'asc');
        setOrderBy(name);
    };

    const deleteProxyClicked = () => {
        selected.forEach((id: number) => deleteAccount(id));
    };

    const isSelected = (id: number) => selected.indexOf(id) !== -1;

    return (
        <div className="accounts">
            <CreateAccount show={show} handleClose={() => setShow(false)} />
            <div className="page-section">
                <div className="table-toolbar">
                    <h3 className="table-toolbar-title">Active accounts</h3>
                    {selected.length > 0 ? (
                        <Tooltip title="Delete">
                            <IconButton onClick={deleteProxyClicked}>
                                <DeleteIcon />
                            </IconButton>
                        </Tooltip>
                    ) : (
                        <Tooltip title="Add">
                            <IconButton onClick={() => setShow(true)}>
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
                            numSelected={selected.length}
                            order={order}
                            orderBy={orderBy}
                            onSelectAllClick={event =>
                                selectAccounts(event.target.checked)
                            }
                            onRequestSort={handleRequestSort}
                            rowCount={accounts.active.length}
                            headCells={headCells}
                        />
                        <TableBody>
                            {stableSort(
                                accounts.active,
                                getComparator(order, orderBy)
                            ).map((a: AccountVm) => {
                                const isRowSelected = isSelected(a.id);

                                return (
                                    <TableRow
                                        key={a.username}
                                        hover
                                        onClick={() => selectAccount(a.id)}
                                        role="checkbox"
                                        selected={isRowSelected}
                                    >
                                        <TableCell padding="checkbox">
                                            <Checkbox checked={isRowSelected} />
                                        </TableCell>
                                        <TableCell>{a.username}</TableCell>
                                        <TableCell>{a.password}</TableCell>
                                        <TableCell>
                                            {getTotalLevel(a.skills)}
                                        </TableCell>
                                        <TableCell>{a.goldEarned}</TableCell>
                                        <TableCell>
                                            {formatRuntime(a.runtimeMinutes)}
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
                        <Typography variant="h6">Trash</Typography>
                    </Toolbar>
                    <Table stickyHeader>
                        <TableHead>
                            <TableRow>
                                <TableCell>Username</TableCell>
                                <TableCell>Password</TableCell>
                                <TableCell>Total Level</TableCell>
                                <TableCell>Gold Earned</TableCell>
                                <TableCell>Runtime</TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {accounts.banned.map(m => (
                                <TableRow key={m.username}>
                                    <TableCell>{m.username}</TableCell>
                                    <TableCell>{m.password}</TableCell>
                                    <TableCell>
                                        {getTotalLevel(m.skills)}
                                    </TableCell>
                                    <TableCell>{m.goldEarned}</TableCell>
                                    <TableCell>
                                        {formatRuntime(m.runtimeMinutes)}
                                    </TableCell>
                                </TableRow>
                            ))}
                        </TableBody>
                    </Table>
                </TableContainer>
            </div>
        </div>
    );
};

const getTotalLevel = (skills: Skill[]) => {
    return skills.map(s => s.level).reduce((a, c) => a + c, 0);
};

const formatRuntime = (runtime: number) => {
    const days = Math.floor(runtime / MINUTES_IN_DAY);
    runtime = Math.floor(runtime % MINUTES_IN_DAY);
    const hours = Math.floor(runtime / MINUTES_IN_HOUR);
    runtime = Math.floor(runtime % MINUTES_IN_HOUR);
    const minutes = Math.floor(runtime % MINUTES_IN_HOUR);

    if (days > 0) {
        return `${days}d ${hours}h ${minutes}m`;
    }

    if (hours > 0) {
        return `${hours}h ${minutes}m`;
    }

    return `${minutes}m`;
};

export default Accounts;
