import React from 'react';
import { withRouter, Link } from 'react-router-dom';
import './SideBar.css';

class SideBar extends React.Component {
    renderListItems() {
        return this.props.entries.map(e => (
            <li key={e.path}>
                <Link to={e.path}>
                    <e.icon />
                    <p>{e.name}</p>
                </Link>
            </li>
        ));
    }

    navigate(path) {
        this.props.history.push(path);
    }

    render() {
        return (
            <nav>
                <ul>{this.renderListItems()}</ul>
            </nav>
        );
    }
}

export default withRouter(SideBar);
