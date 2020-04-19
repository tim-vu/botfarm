import React from 'react';
import TableHead from '@material-ui/core/TableHead';
import TableRow from '@material-ui/core/TableRow';
import TableCell from '@material-ui/core/TableCell';
import TableSortLabel from '@material-ui/core/TableSortLabel';
import Checkbox from '@material-ui/core/Checkbox';

export type Order = 'asc' | 'desc';

export interface HeadCell<T> {
    name: (keyof T & string) | (keyof T & number);
    label: string;
    disablePadding: boolean;
    numeric: boolean;
}

interface EnhancedTableHeadProps<T> {
    headCells: HeadCell<T>[];
    numSelected: number;
    order: Order;
    orderBy: keyof T;
    rowCount: number;
    onRequestSort: (
        event: React.MouseEvent<unknown>,
        property: keyof T
    ) => void;
    onSelectAllClick: (
        event: React.ChangeEvent<HTMLInputElement>,
        checked: boolean
    ) => void;
}

type IEnhancedTableHead<T = any> = React.FC<EnhancedTableHeadProps<T>>;

const EnhancedTableHead: <T extends {}>(
    props: EnhancedTableHeadProps<T>
) => React.ReactElement<EnhancedTableHeadProps<T>> = props => {
    const {
        headCells,
        order,
        orderBy,
        onSelectAllClick,
        numSelected,
        rowCount,
        onRequestSort
    } = props;

    return (
        <TableHead>
            <TableRow>
                <TableCell padding="checkbox">
                    <Checkbox
                        indeterminate={
                            numSelected > 0 && numSelected < rowCount
                        }
                        checked={rowCount > 0 && numSelected === rowCount}
                        onChange={onSelectAllClick}
                    />
                </TableCell>
                {headCells.map(headCell => (
                    <TableCell
                        key={headCell.label}
                        padding={headCell.disablePadding ? 'none' : 'default'}
                        sortDirection={
                            orderBy === headCell.name ? order : false
                        }
                    >
                        <TableSortLabel
                            active={orderBy === headCell.name}
                            direction={
                                orderBy === headCell.name ? order : 'asc'
                            }
                            onClick={event =>
                                onRequestSort(event, headCell.name)
                            }
                        >
                            {headCell.label}
                        </TableSortLabel>
                    </TableCell>
                ))}
            </TableRow>
        </TableHead>
    );
};

export default EnhancedTableHead;
