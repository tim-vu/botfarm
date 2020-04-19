import React from 'react';
import IconButton from '@material-ui/core/IconButton';
import NotificationsIcon from '@material-ui/icons/Notifications';
import ExitToAppIcon from '@material-ui/icons/ExitToApp';
import './Header.css';

class Header extends React.Component {
    render() {
        return (
            <div className="header">
                <div className="icons">
                    <IconButton color="inherit" size="medium">
                        <NotificationsIcon />
                    </IconButton>
                    <IconButton color="inherit" size="medium">
                        <ExitToAppIcon />
                    </IconButton>
                </div>
            </div>
        );
    }
}

export default Header;
