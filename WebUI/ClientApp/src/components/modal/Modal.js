import React from 'react';
import './Modal.css';

export default ({ show, children, modalClassName }) => {
    const showHideClassName = show
        ? 'modal display-block'
        : 'modal display-none';

    return (
        <div className={showHideClassName}>
            <div className={modalClassName}>{children}</div>
        </div>
    );
};
