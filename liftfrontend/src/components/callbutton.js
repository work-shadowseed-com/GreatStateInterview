import React, { Component } from 'react';

class CallButton extends Component {

    constructor(props) {
        super(props);
        this.state = {
            called: "",
            currentFloor: 0
        }

        this.callLift = this.callLift.bind(this);
        this.liftCalled = this.liftCalled.bind(this);
        this.textUpdate = this.textUpdate.bind(this);
    }

    callLift() {
        let floor = this.state.currentFloor;
        fetch("https://localhost:5001/api/call", 
            {
                method: "POST",
                mode: "cors",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(floor)
            }
        ).then(() => {
            this.liftCalled()
        });
    }

    liftCalled() {
        this.setState({
            called: "called"
        });
    }
    
    textUpdate(event) {
        this.setState({
            currentFloor: event.target.value
        });
    }

    render() {
        
        return (
            <div className="lift">
                <p>Current floor: <input type="text" onChange={this.textUpdate}></input><br/><br/>
                <button className={this.state.called} onClick={this.callLift}>Call lift</button></p>
            </div>
        );
    }
}

export default CallButton;