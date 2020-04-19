import './App.css';
import Header from '../header/Header';
import SideBar from '../sidebar/SideBar';
import Overview from '../pages/overview/Overview';
import Accounts from '../pages/accounts/Accounts';
import Proxies from '../pages/proxies/Proxies';
import Logs from '../pages/logs/Logs';
import Settings from '../pages/settings/Settings';
import { useInterval } from '../useInterval';
import React from 'react';
import { BrowserRouter as Router, Switch, Route } from 'react-router-dom';
import AssessmentIcon from '@material-ui/icons/Assessment';
import AccountBoxIcon from '@material-ui/icons/AccountBox';
import RssFeedIcon from '@material-ui/icons/RssFeed';
import ListAltIcon from '@material-ui/icons/ListAlt';
import SettingsIcon from '@material-ui/icons/Settings';
import { useStoreActions } from 'easy-peasy';

const ROUTES = [
    {
        name: 'Overview',
        icon: AssessmentIcon,
        path: '/',
        exact: true,
        component: Overview
    },
    {
        name: 'Accounts',
        icon: AccountBoxIcon,
        path: '/accounts',
        component: Accounts
    },
    {
        name: 'Proxies',
        icon: RssFeedIcon,
        path: '/proxies',
        component: Proxies
    },
    {
        name: 'Logs',
        icon: ListAltIcon,
        path: '/logs',
        component: Logs
    },
    {
        name: 'Settings',
        icon: SettingsIcon,
        path: '/settings',
        component: Settings
    }
];

const INTERVAL = 5000;

const App = () => {
    const fetchAccounts = useStoreActions(state => state.account.fetchAccounts);
    const fetchProxies = useStoreActions(state => state.proxy.fetchProxies);
    const fetchStatus = useStoreActions(state => state.status.fetchStatus);

    useInterval(() => {
        fetchAccounts();
        fetchProxies();
        fetchStatus();
    }, INTERVAL);

    return (
        <Router>
            <div className="page">
                <h1>FORFarm</h1>
                <div className="header">
                    <Header />
                </div>
                <div className="sidebar">
                    <SideBar entries={ROUTES} />
                </div>
                <div className="content">
                    <Switch>
                        {ROUTES.map((route, index) => (
                            <Route
                                key={index}
                                path={route.path}
                                exact={route.exact}
                                component={route.component}
                            />
                        ))}
                    </Switch>
                </div>
            </div>
        </Router>
    );
};

export default App;
