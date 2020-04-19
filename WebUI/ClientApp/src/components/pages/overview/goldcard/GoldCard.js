import React from 'react';
import Card from '@material-ui/core/Card';
import CardContent from '@material-ui/core/CardContent';
import './GoldCard.css';

class GoldCard extends React.Component {

    render() {
        return <Card className="card">
            <CardContent className="cardContent">
                <h4>{this.props.title}</h4>
                <p>{this.formatAmount(this.props.amount)}</p>
            </CardContent>
        </Card>
    }

    formatAmount(amount){

        const MILLION = 1000 * 1000;
        const THOUSAND = 1000;

        if(amount > MILLION){
            return (amount / MILLION).toLocaleString(undefined, {minimumFractionDigits: 1}) + "m"
        }

        return (amount / THOUSAND).toLocaleString(undefined, {minimumFractionDigits: 1}) + "k";

    }

}

export default GoldCard;