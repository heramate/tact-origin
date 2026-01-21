using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace RACTTerminal
{
    public class TelnetParser
    {
        public event NegotiateParserEventHandler NvtParserEvent;
        private States m_State = States.Ground;
        private Char m_CurrentChar = '\0';
        private string m_CurrentSequence = "";

        private ArrayList m_ParamList = new ArrayList();
        private CharEvents m_CharEvents = new CharEvents();
        private uc_StateChangeEvents m_StateChangeEvents = new uc_StateChangeEvents();
        private Params m_CurrentParams = new Params();

        public TelnetParser()
        {
        }

        public void Parsestring(string aInstring)
        {
            States tNextState = States.None;
            E_NegotiateActions tNextAction = E_NegotiateActions.None;
            E_NegotiateActions tStateExitAction = E_NegotiateActions.None;
            E_NegotiateActions tStateEntryAction = E_NegotiateActions.None;

            foreach (Char tChar in aInstring)
            {
                this.m_CurrentChar = tChar;

                m_CharEvents.GetStateEventAction(m_State, m_CurrentChar, ref tNextState, ref tNextAction);

                if (tNextState != States.None && tNextState != this.m_State)
                {
                    m_StateChangeEvents.GetStateChangeAction(this.m_State, Transitions.Exit, ref tStateExitAction);
                    if (tStateExitAction != E_NegotiateActions.None) DoAction(tStateExitAction);
                }

                if (tNextAction != E_NegotiateActions.None) DoAction(tNextAction);

                if (tNextState != States.None && tNextState != this.m_State)
                {
                    this.m_State = tNextState;
                    m_StateChangeEvents.GetStateChangeAction(this.m_State, Transitions.Entry, ref tStateExitAction);
                    if (tStateEntryAction != E_NegotiateActions.None) DoAction(tStateEntryAction);
                }
            }
        }

        private void DoAction(E_NegotiateActions NextAction)
        {
            switch (NextAction)
            {
                case E_NegotiateActions.Dispatch:
                case E_NegotiateActions.Collect:
                    this.m_CurrentSequence += m_CurrentChar.ToString();
                    break;

                case E_NegotiateActions.NewCollect:
                    this.m_CurrentSequence = m_CurrentChar.ToString();
                    this.m_CurrentParams.Clear();
                    break;

                case E_NegotiateActions.Param:
                    this.m_CurrentParams.Add(m_CurrentChar);
                    break;

                default:
                    break;
            }

            // send the external event requests
            switch (NextAction)
            {
                case E_NegotiateActions.Dispatch:

                    //                    System.Console.Write ("Sequence = {0}, Char = {1}, PrmCount = {2}, State = {3}, NextAction = {4}\n",
                    //                        this.CurSequence, (int) this.CurChar, this.CurParams.Count (), 
                    //                        this.State, NextAction);

                    NvtParserEvent(this, new NegotiateParserEventArgs(NextAction, m_CurrentChar, m_CurrentSequence, m_CurrentParams));
                    break;

                case E_NegotiateActions.Execute:
                case E_NegotiateActions.SendUp:
                    NvtParserEvent(this, new NegotiateParserEventArgs(NextAction, m_CurrentChar, m_CurrentSequence, m_CurrentParams));

                    //                    System.Console.Write ("Sequence = {0}, Char = {1}, PrmCount = {2}, State = {3}, NextAction = {4}\n",
                    //                        this.CurSequence, (int) this.CurChar, this.CurParams.Count (), 
                    //                        this.State, NextAction);
                    break;
                default:
                    break;
            }


            switch (NextAction)
            {
                case E_NegotiateActions.Dispatch:
                    this.m_CurrentSequence = "";
                    this.m_CurrentParams.Clear();
                    break;

                default:
                    break;
            }
        }

        private enum States
        {
            None = 0,
            Ground = 1,
            Command = 2,
            Anywhere = 3,
            Synch = 4,
            Negotiate = 5,
            SynchNegotiate = 6,
            SubNegotiate = 7,
            SubNegValue = 8,
            SubNegParam = 9,
            SubNegEnd = 10,
            SynchSubNegotiate = 11,
        }

        private enum Transitions
        {
            None = 0,
            Entry = 1,
            Exit = 2,
        }

        private struct uc_StateChangeInfo
        {
            public States State;
            public Transitions Transition;    
            public E_NegotiateActions NextAction;

            public uc_StateChangeInfo(States p1,Transitions p2,E_NegotiateActions p3)
            {
                this.State = p1;
                this.Transition = p2;
                this.NextAction = p3;
            }
        }

        private class uc_StateChangeEvents
        {
            private uc_StateChangeInfo[] Elements = 
			{
				new uc_StateChangeInfo (States.None, Transitions.None, E_NegotiateActions.None),
				};

            public uc_StateChangeEvents()
            {
            }

            public Boolean GetStateChangeAction(States State,Transitions Transition,ref E_NegotiateActions NextAction)
            {
                uc_StateChangeInfo Element;

                for (System.Int32 i = 0; i < Elements.Length; i++)
                {
                    Element = Elements[i];

                    if (State == Element.State &&
                        Transition == Element.Transition)
                    {
                        NextAction = Element.NextAction;
                        return true;
                    }
                }

                return false;
            }
        }

        private struct CharEventInfo
        {
            public States CurState;
            public Char CharFrom;
            public Char CharTo;
            public E_NegotiateActions NextAction;
            public States NextState;  

            public CharEventInfo(States aStates,Char aCharFrom,Char aCharTo,E_NegotiateActions aNextAction,States aNextState)
            {
                this.CurState = aStates;
                this.CharFrom = aCharFrom;
                this.CharTo = aCharTo;
                this.NextAction = aNextAction;
                this.NextState = aNextState;
            }
        }

        private class CharEvents
        {
            public Boolean GetStateEventAction(States aCurrentState,System.Char aCurrentChar,ref States aNextState,ref E_NegotiateActions aNextAction)
            {
                CharEventInfo tElement;

                for (Int32 i = 0; i < CharEvents.Elements.Length; i++)
                {
                    tElement = CharEvents.Elements[i];

                    if (aCurrentChar >= tElement.CharFrom &&
                        aCurrentChar <= tElement.CharTo &&
                        (aCurrentState == tElement.CurState || tElement.CurState == States.Anywhere))
                    {
                        aNextState = tElement.NextState;
                        aNextAction = tElement.NextAction;
                        return true;
                    }
                }

                return false;
            }

            public CharEvents()
            {
            }

            public static CharEventInfo[] Elements = 
			{
				new CharEventInfo (States.Ground,       (char) 000, (char) 254, E_NegotiateActions.SendUp,     States.None),
				new CharEventInfo (States.Ground,       (char) 255, (char) 255, E_NegotiateActions.None,       States.Command),
				new CharEventInfo (States.Command,      (char) 000, (char) 239, E_NegotiateActions.SendUp,     States.Ground),
				new CharEventInfo (States.Command,      (char) 240, (char) 241, E_NegotiateActions.None,       States.Ground),
				new CharEventInfo (States.Command,      (char) 242, (char) 249, E_NegotiateActions.Execute,    States.Ground),
				new CharEventInfo (States.Command,      (char) 250, (char) 250, E_NegotiateActions.NewCollect, States.SubNegotiate),
				new CharEventInfo (States.Command,      (char) 251, (char) 254, E_NegotiateActions.NewCollect, States.Negotiate),
				new CharEventInfo (States.Command,      (char) 255, (char) 255, E_NegotiateActions.SendUp,     States.Ground),
				new CharEventInfo (States.SubNegotiate, (char) 000, (char) 255, E_NegotiateActions.Collect,    States.SubNegValue),
				new CharEventInfo (States.SubNegValue,  (char) 000, (char) 001, E_NegotiateActions.Collect,    States.SubNegParam),
				new CharEventInfo (States.SubNegValue,  (char) 002, (char) 255, E_NegotiateActions.None,       States.Ground),
				new CharEventInfo (States.SubNegParam,  (char) 000, (char) 254, E_NegotiateActions.Param,      States.None),
				new CharEventInfo (States.SubNegParam,  (char) 255, (char) 255, E_NegotiateActions.None,       States.SubNegEnd),
				new CharEventInfo (States.SubNegEnd,    (char) 000, (char) 239, E_NegotiateActions.None,       States.Ground),
				new CharEventInfo (States.SubNegEnd,    (char) 240, (char) 240, E_NegotiateActions.Dispatch,   States.Ground),
				new CharEventInfo (States.SubNegEnd,    (char) 241, (char) 254, E_NegotiateActions.None,       States.Ground),
				new CharEventInfo (States.SubNegEnd,    (char) 255, (char) 255, E_NegotiateActions.Param,      States.SubNegParam),
				new CharEventInfo (States.Negotiate,    (char) 000, (char) 255, E_NegotiateActions.Dispatch,   States.Ground),
				};
        }
    }
}
